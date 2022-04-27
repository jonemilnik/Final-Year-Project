using UnityEngine;

public class AgentNavigatingState : AgentBaseState
{
    public override void EnterState(AgentStateManager manager)
    {
        Transform goalPoint = GameObject.Find("GoalState").transform;
        manager.agent.SetDestination(goalPoint.position);
    }

    public override void UpdateState(AgentStateManager manager)
    {
        //If enemies nearby transition to running state
        if (manager.playerHandler.nearbyEnemies.Count > 0)
        {
            bool transition = false;

            foreach (var enemy in manager.playerHandler.nearbyEnemies)
            {
                float distToEnemy = Vector3.Distance(enemy.transform.position, manager.transform.position);
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController.isFacingPlayer || distToEnemy <= enemyController.fov.viewRadius + 2f)
                {
                    transition = true;
                }
            }

            if (transition)
            {
                manager.TransitionState(manager.runningState);
            }
           
        }
    }

}
