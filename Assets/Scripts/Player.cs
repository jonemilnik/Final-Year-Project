using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    public bool isHiding = false;
    private Vector3 prevPos;
    public float thrust = 1.2f;
    [SerializeField]
    private List<Transform> _waypoints;

    void CheckMovementInput() 
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.forward * thrust);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.back * thrust);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * thrust);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left * thrust);
        }
    }
    
    void HidePlayer(Collider collider)
    {
        if (!isHiding)
        {
            isHiding = true;
            prevPos = rb.position;
            Vector3 colliderPos = collider.bounds.center;
            rb.transform.position = new Vector3(colliderPos.x, colliderPos.y + 0.25f, colliderPos.z);
        } else
        {
            isHiding = false;
            rb.transform.position = prevPos;
        }
       
    }

    Collider[] GetInActiveRadius(float radius)
    {
        return Physics.OverlapSphere(rb.position, radius);
    }


    void CheckActionInput()
    {
        Collider[] hitColliders = GetInActiveRadius(2.5f);

        if (Input.GetKeyDown(KeyCode.E) && hitColliders.Length != 0)
        {
            foreach(Collider collider in hitColliders)
            {
                if (collider.gameObject.CompareTag("Bin"))
                {
                    HidePlayer(collider);
                    break;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(_waypoints[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementInput();
        CheckActionInput();

    }
}
