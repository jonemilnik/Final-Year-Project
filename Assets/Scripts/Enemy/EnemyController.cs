using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    private int waypointPointer = 0;
    [HideInInspector]
    public bool isWalking;
    [HideInInspector]
    public bool isInspecting;
    [SerializeField]
    private List<Transform> _waypoints;
    //Designates rotation when inspecting area
    [SerializeField]
    private float inspectAngle;
    //Initial transform forward at start of inspecting behaviour
    private Vector3 initialForward;
    [HideInInspector]
    public bool isFacingPlayer = false;
    FieldOfView fov;

    void UpdateFacingPlayer()
    {
        PlayerHandler player = GameObject.Find("Player").GetComponent<PlayerHandler>();
        Vector3 dirToPlayer = player.transform.position - transform.position;

        //Set direction to hideable waypoint as player can exit to that spot
        if (player.isHiding)
        {
            dirToPlayer = player.prevPos - transform.position;
        }
        
        Vector3 forward = transform.forward;

        //Forward used is different when enemy inspecting
        if (isInspecting)
        {
            forward = initialForward;
        }

        //If player can be spotted when enemy inspecting
        if (Vector3.Angle(forward, dirToPlayer) <= inspectAngle + fov.viewAngle / 2)
        {
            isFacingPlayer = true;
        } else
        {
            isFacingPlayer = false;
        }
    }

    public float GetDistToPlayer()
    {
        Transform player = GameObject.Find("Player").transform;
        return Vector3.Distance(transform.position, player.position);
    }

    public float GetDistToNextWaypoint()
    {
        return agent.remainingDistance;
    }

    IEnumerator InspectArea() 
    {
        isInspecting = true; 

        //Face path first
        yield return StartCoroutine(FacePath());

        //Inspect Left
        yield return StartCoroutine(InspectDirection(-inspectAngle, 0.05f, 0.15f));
        
        //Inspect Right
        yield return StartCoroutine(InspectDirection(inspectAngle * 2, 0.05f, 0.15f));

        //Centre view again
        yield return StartCoroutine(InspectDirection(-inspectAngle, 0.05f, 0.04f));

        isInspecting = false;
    }

    IEnumerator InspectDirection(float angle, float rotationSpeed, float timeLimit)
    {
        
        //Angle to look from forward direction
        float time = 0.0f;

        Vector3 inspectDir = fov.GetVectorFromAngle(angle);
        Quaternion rotation = Quaternion.LookRotation(inspectDir);

        while (time < 0.15f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, time);
            time += Time.deltaTime * rotationSpeed;
            yield return null;
        }
    }

    IEnumerator FacePath()
    {
        Vector3 dir = GetDirToPath();

        //Store initial forward - used for checking if facing player
        initialForward = dir;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        float time = 0.0f;
        float rotationSpeed = 0.05f;

        while (time < 0.03f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * rotationSpeed;
            yield return null;
        }
    }


    //Get direction to face path
    Vector3 GetDirToPath()
    {
        //Used to filter only colliders with 'Path' layer
        int layerMask = 1 << 3;
        //Perform raycast in these directions
        Vector3[] directions =
            {
                Vector3.forward, Vector3.back, Vector3.left, Vector3.right
            };

        //Stores index of direction and value of distance to path
        (int?, float?) closestDir = (null, null);

        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, directions[i], out hit, fov.viewRadius, layerMask))
            {
                if (closestDir.Item1 == null)
                {
                    closestDir = (i, hit.distance);
                }
                else
                {
                    //Direction of raycast has shorter distance to path
                    if (hit.distance < closestDir.Item2)
                    {
                        closestDir = (i, hit.distance);
                    }
                }
            }
        }
        return directions[(int)closestDir.Item1];
        //transform.rotation = Quaternion.LookRotation(directions[(int) closestDir.Item1]);
    }

    private void Awake()
    {
        // Initialize initial state
        agent = GetComponent<NavMeshAgent>();
        fov = transform.GetChild(0).GetComponent<FieldOfView>();

    }

    // Start is called before the first frame update
    void Start()
    {
        //Walk to first waypoint
        agent.SetDestination(_waypoints[waypointPointer].position);
        isWalking = true;
        isInspecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFacingPlayer();
        fov.FindPlayer();

        // Inspecting area
        if (!isWalking)
        {
            
            if (!isInspecting)
            {
                isWalking = true;
                waypointPointer = (waypointPointer + 1) % _waypoints.Count;
                agent.SetDestination(_waypoints[waypointPointer].position);
            }
        }
        else
        {
            // Normalises vectors to worry only about x and z values
            Vector3 normalVector = new Vector3(1, 0, 1);

            // Checks if normal vectors distance is less than epsilon
            if (Vector3.Distance(Vector3.Scale(gameObject.transform.position, normalVector)
                , Vector3.Scale(_waypoints[waypointPointer].position, normalVector)) < Vector3.kEpsilon)
            {
                isWalking = false;
                StartCoroutine(InspectArea());
            }
        }
    }

}
