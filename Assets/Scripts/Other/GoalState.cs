using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalState : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Player")
        {
            Debug.Log("Reached Goal!");
        }
    }

}
