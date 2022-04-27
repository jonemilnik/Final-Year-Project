using UnityEngine;

public abstract class AgentBaseState 
{
    public abstract void EnterState(AgentStateManager agent);

    public abstract void UpdateState(AgentStateManager agent);


}
