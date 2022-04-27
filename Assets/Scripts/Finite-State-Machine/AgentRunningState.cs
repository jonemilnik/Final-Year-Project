using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentRunningState : AgentBaseState
{
    private GameObject nearestBin = null;

    public override void EnterState(AgentStateManager manager)
    {
        List<GameObject> objects = manager.playerHandler.GetListInActiveRadius(10.0f);
        Transform player = manager.playerHandler.transform;
        float nearestDist = Mathf.Infinity;

        //Find nearest bin in list of nearby objects
        for (int i = 0; i < objects.Count; i++)
        {
            //Distance between player and current bin in iteration
            float distance = Vector3.Distance(player.position, objects[i].transform.position);

            if (string.Equals(objects[i].tag, "Bin") && distance < nearestDist)
            {
                nearestBin = objects[i];
                nearestDist = distance;
            }
        }

        manager.agent.SetDestination(nearestBin.transform.position);
    }

    public override void UpdateState(AgentStateManager manager)
    {
        Transform player = manager.playerHandler.transform;

        //Player is within 1 game unit of the hideable waypoint location
        if ( Vector3.Distance(player.position, nearestBin.transform.position) <= 1 )
        {
            manager.TransitionState(manager.hidingState);
            nearestBin = null;
        }

        

    }

}
