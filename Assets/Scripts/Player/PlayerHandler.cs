using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerHandler : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    private Vector3 prevPos;
    public float thrust = 1.2f;
    [SerializeField]
    private List<Transform> _waypoints;
    [HideInInspector]
    public bool isSpotted = false;
    [HideInInspector]
    public bool isRunning = false;
    [HideInInspector]
    public bool isHiding = false;

    public void setIsSpotted (bool b)
    {
        isSpotted = b;
    }

    
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

    public void Hide()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4.0f);
        Collider closestBin = null;
        float closestDist = Mathf.Infinity;

        //Find closest bin in radius of 4
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];
            Debug.Log(collider.name);
            float distance = Vector3.SqrMagnitude(transform.position - collider.transform.position);

            if (collider.name == "Bin" && distance <= closestDist)
            {
                closestBin = collider;
            }
        }

        Vector3 binPos = closestBin.bounds.center;
        agent.enabled = false;
        transform.position = new Vector3(binPos.x, binPos.y + 0.25f, binPos.z);
        prevPos = transform.position;
        isHiding = true;

    }

    void HidePlayer(Collider collider)
    {
        if (!isHiding)
        {
            isHiding = true;
            prevPos = rb.position;
            Vector3 colliderPos = collider.bounds.center;
            rb.transform.position = new Vector3(colliderPos.x, colliderPos.y + .25f, colliderPos.z);
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
    void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void Start()
    {
        //transform.position = GameObject.Find("StartState").GetComponent<Transform>().position;
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("EnemyWalls").GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Player").GetComponent<Collider>());
        //Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Map").GetComponent<Collider>());
        //Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Bin2").GetComponent<Collider>());


    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = agent.nextPosition;
        //CheckMovementInput();
        //CheckActionInput();
    }
}
