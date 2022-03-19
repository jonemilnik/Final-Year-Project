using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private int waypointPointer = 0;
    private bool isWalking;
    private bool isInspecting;
    //[SerializeField]
    //private float waitTime;
    [SerializeField]
    private List<Transform> _waypoints;
    [SerializeField]
    private float inspectAngle;

    IEnumerator InspectArea() 
    {
        isInspecting = true; 

        //Face path first
        yield return StartCoroutine("FacePath");

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
        FieldOfView fov = transform.GetChild(0).GetComponent<FieldOfView>();
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
        FieldOfView fov = transform.GetChild(0).GetComponent<FieldOfView>();
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

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize initial state
        agent = GetComponent<NavMeshAgent>();
        isWalking = true;
        isInspecting = false;
        //Walk to first waypoint
        //agent.SetDestination(_waypoints[waypointPointer].position);
    }

    // Update is called once per frame
    void Update()
    {

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
