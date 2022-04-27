using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentStateManager : MonoBehaviour
{
    AgentBaseState currentState;
    public AgentRunningState runningState = new AgentRunningState();
    public AgentNavigatingState navigatingState = new AgentNavigatingState();
    public AgentHidingState hidingState = new AgentHidingState();
    public AgentLeaveHidingState leaveHidingState = new AgentLeaveHidingState();
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public PlayerHandler playerHandler;

    // Start is called before the first frame update
    void Awake()
    {
        playerHandler = GetComponent<PlayerHandler>();
        agent = GetComponent<NavMeshAgent>();

        currentState = navigatingState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void TransitionState(AgentBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
