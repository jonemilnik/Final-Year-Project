﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.Semantic.Traits;
using Unity.AI.Planner.Controller;


public class EnemyTraitController : MonoBehaviour
{
    Enemy enemyTrait;
    DecisionController decisionController;
    // Start is called before the first frame update
    void Start()
    {
        enemyTrait = GetComponent<Enemy>();
        decisionController = GetComponent<DecisionController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        EnemyController controller = GetComponent<EnemyController>();
        enemyTrait.DistToPlayer = controller.GetDistToPlayer();
        enemyTrait.IsFacingPlayer = controller.isFacingPlayer;
        
        //enemyTrait.IsFacingPlayer = controller.isFacingPlayer;
        //Debug.Log("Enemy facing player: " + enemyTrait.IsFacingPlayer);
    }
}
