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
    private List<Vector3> waypoints = new List<Vector3>();
    void SetWaypoints()
    {

        foreach (Transform child in transform)
        {
            waypoints.Add(child.position);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize initial state
        agent = GetComponent<NavMeshAgent>();
        isWalking = false;
        SetWaypoints();
        
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
                waypointPointer = (waypointPointer + 1) % waypoints.Count;
                agent.SetDestination(waypoints[waypointPointer]);
                Debug.Log(waypoints[waypointPointer]);
            }
        }
        else
        {

            // Normalises vectors to worry only about x and z values
            Vector3 normalVector = new Vector3(1, 0, 1);

            // Checks if normal vectors distance is less than epsilon
            if (Vector3.Distance(Vector3.Scale(gameObject.transform.position, normalVector)
                , Vector3.Scale(waypoints[waypointPointer], normalVector)) < Vector3.kEpsilon)
            {
                isWalking = false;
            }
        }
    }

}
