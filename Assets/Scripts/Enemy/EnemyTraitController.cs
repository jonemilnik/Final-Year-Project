using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.Semantic.Traits;
using Unity.AI.Planner.Controller;


public class EnemyTraitController : MonoBehaviour
{
    Enemy enemyTrait;
    Mover moverTrait;
    EnemyController controller;

    // Start is called before the first frame update
    void Start()
    {
        enemyTrait = GetComponent<Enemy>();
        moverTrait = GetComponent<Mover>();
        controller = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyTrait.DistToPlayer = controller.GetDistToPlayer();
        enemyTrait.DistToWaypoint = controller.GetDistToNextWaypoint();
        enemyTrait.IsFacingPlayer = controller.isFacingPlayer;
        enemyTrait.Speed = controller.agent.speed;

        Vector3 nextWaypoint = controller.GetNextWaypoint();
        enemyTrait.WaypointX = nextWaypoint.x;
        enemyTrait.WaypointY = nextWaypoint.y;
        enemyTrait.WaypointZ = nextWaypoint.z;


        moverTrait.X = transform.position.x;
        moverTrait.Y = transform.position.y;
        moverTrait.Z = transform.position.z;

        moverTrait.ForwardX = transform.forward.x;
        moverTrait.ForwardY = transform.forward.y;
        moverTrait.ForwardZ = transform.forward.z;

    }
}
