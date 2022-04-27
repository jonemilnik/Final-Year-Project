using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerHandler : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    [HideInInspector]
    public Vector3 prevPos;
    public float thrust = 1.2f;
    [SerializeField]
    private List<Transform> _waypoints;
    [HideInInspector]
    public bool isSpotted = false;
    [HideInInspector]
    public bool isRunning = false;
    [HideInInspector]
    public bool isHiding = false;
    [HideInInspector]
    public List<GameObject> nearbyEnemies;

    public void setIsSpotted (bool b)
    {
        isSpotted = b;
    }

    public void Navigate(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
    public void Hide()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5.0f);
        Collider closestBin = null;
        float closestDist = Mathf.Infinity;
        //Find closest bin in radius of 4
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];
            float distance = Vector3.SqrMagnitude(transform.position - collider.transform.position);
            if (collider.name == "Bin" && distance <= closestDist)
            {
                closestBin = collider;
                
            }
        }
        Vector3 binPos = closestBin.bounds.center;
        agent.enabled = false;
        prevPos = transform.position;
        transform.position = new Vector3(binPos.x, binPos.y + 0.25f, binPos.z);
        isHiding = true;

    }

    public void StopHiding()
    {
        transform.position = prevPos;
        isHiding = false;
        agent.enabled = true;
    }


    public List<GameObject> GetListInActiveRadius(float radius)
    {
        List<GameObject> objects = new List<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(rb.position, radius);
        foreach (Collider collider in colliders)
        {
            objects.Add(collider.gameObject);
        }

        return objects;
    }

    void UpdateNearbyEnemies()
    {
        List<GameObject> colliders = GetListInActiveRadius(7f);
        for (int i = 0; i < colliders.Count; i++)
        {
            //Add only new enemy colliders
            if (string.Equals(colliders[i].tag, "Enemy") && !nearbyEnemies.Contains(colliders[i]))
            {
                nearbyEnemies.Add(colliders[i]);
            }

        }

        //Remove enemies out of range
        for (int i = 0; i < nearbyEnemies.Count; i++)
        {
            if (!colliders.Contains(nearbyEnemies[i]) )
            {
                nearbyEnemies.Remove(nearbyEnemies[i]);
            }
        }
        
    }



    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        nearbyEnemies = new List<GameObject>();
    }

    private void Start()
    {
        //Prevents unusual behaviour between NavMeshAgent and RigidBody components
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("EnemyWalls").GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Player").GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {   //Constantly update nearby Enemies
        UpdateNearbyEnemies();
    }
}
