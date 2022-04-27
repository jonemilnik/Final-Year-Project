using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.Semantic.Traits;
using Unity.AI.Planner.Controller;

public class PlayerTraitController : MonoBehaviour
{
    Player playerTrait;
    Mover moverTrait;
    PlayerHandler playerHandler;

    // Start is called before the first frame update
    void Start()
    {
        playerTrait = GetComponent<Player>();
        moverTrait = GetComponent<Mover>();
        playerHandler = GetComponent<PlayerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        playerTrait.IsSpotted = playerHandler.isSpotted;
        playerTrait.IsHiding = playerHandler.isHiding;
        playerTrait.IsRunning = playerHandler.isRunning;
        moverTrait.X = transform.position.x;
        moverTrait.Y = transform.position.y;
        moverTrait.Z = transform.position.z;

        moverTrait.ForwardX = transform.forward.x;
        moverTrait.ForwardY = transform.forward.y;
        moverTrait.ForwardZ = transform.forward.z;
    }
}
