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
        enemyTrait.Speed = controller.agent.velocity.magnitude;
        //Debug.Log("Enemy speed: " + enemyTrait.Speed);

        moverTrait.X = transform.position.x;
        moverTrait.Y = transform.position.y;
        moverTrait.Z = transform.position.z;

        moverTrait.ForwardX = transform.forward.x;
        moverTrait.ForwardY = transform.forward.y;
        moverTrait.ForwardZ = transform.forward.z;

        //enemyTrait.IsFacingPlayer = controller.isFacingPlayer;
        //Debug.Log("Enemy facing player: " + enemyTrait.IsFacingPlayer);
    }
}
