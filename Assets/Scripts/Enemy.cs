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
    private float timer = 0.0f;
    public float waitTime;
    [SerializeField]
    private List<Transform> _waypoints;

    //IEnumerator InspectArea()
    //{
    //    bool facingPath = false;
    //    Vector3 dir = GetDirToPath();
    //    while (!facingPath)
    //    {

    //    }
    //}

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize initial state
        agent = GetComponent<NavMeshAgent>();
        isWalking = false;
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
            Debug.Log(closestDir);

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

    // Update is called once per frame
    void Update()
    {

        // Inspecting area
        if (!isWalking)
        {
            timer += Time.deltaTime;

            if (timer > waitTime)
            {
                isWalking = true;
                timer = 0.0f;
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
                //FacePath();
            }
        }
    }

}
