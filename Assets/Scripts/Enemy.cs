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

    

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize initial state
        agent = GetComponent<NavMeshAgent>();
        isWalking = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> waypoints = GetWaypoints();

        // Inspecting area
        if (!isWalking)
        {
            timer += Time.deltaTime;

            if (timer > waitTime)
            {
                isWalking = true;
                timer = 0.0f;
                waypointPointer = (waypointPointer + 1) % waypoints.Count;
                agent.SetDestination(waypoints[waypointPointer].transform.position);
            }
        }
        else
        {
            if ( Vector3.SqrMagnitude(gameObject.transform.position - waypoints[waypointPointer].transform.position) < 0.005 == true)  {
                isWalking = false;
            }
        }
    }

    private List<GameObject> GetWaypoints()
    {
        List<GameObject> waypoints = new List<GameObject>();

        foreach (Transform child in transform)
        {
            waypoints.Add(child.gameObject);
        }

        return waypoints;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    List<GameObject> waypoints = GetWaypoints();
 
    //    foreach (GameObject wp in waypoints)
    //    {
    //        wp.GetComponent<Waypoint>().setParentSelected(true);
    //    }
    //}
}
