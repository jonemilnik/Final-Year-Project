using UnityEngine;

public class AgentHidingState : AgentBaseState
{
    public override void EnterState(AgentStateManager manager)
    {
        manager.playerHandler.Hide();
    }

    public override void UpdateState(AgentStateManager manager)
    {
        // Transition if no enemies nearby
        if (manager.playerHandler.nearbyEnemies.Count == 0)
        {
            manager.playerHandler.StopHiding();
            manager.TransitionState(manager.navigatingState);
            
        }
    }


}
