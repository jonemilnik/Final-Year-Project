using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalState : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Player")
        {
            FindObjectOfType<GameManager>().WinGame();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
