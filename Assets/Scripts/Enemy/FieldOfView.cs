using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
    public Material material;

    //Mesh rendering
    private Mesh mesh;
    private Vector3 leftMostVector;
    private Vector3 rightMostVector;

    // Number of triangles rendered per degree
    public int meshResolution;

    private void Start()
    {
        //Vectors for fov vision
        leftMostVector = GetVectorFromAngle(-viewAngle / 2) * viewRadius;
        rightMostVector = GetVectorFromAngle(viewAngle / 2) * viewRadius;

        //Initialising fov mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        DrawFieldOfView();
        
        //Setting material
        Color color = material.color;
        color.a = 0.2f;
        material.color = color;
        GetComponent<MeshRenderer>().material = material;

    }

    //IEnumerator FindPlayerWithDelay(float delay)
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(delay);
    //        FindPlayer();
    //    }
    //}

    void DrawFieldOfView()
    {
        //No. of triangles to render
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngle = viewAngle / stepCount;
        Vector3[] vertices = new Vector3[2 + stepCount];
        int[] triangles = new int[3 * stepCount];
        Vector2[] uvs = new Vector2[vertices.Length];

        vertices[0] = Vector3.zero;
        vertices[1] = leftMostVector;
        uvs[0] = new Vector2(vertices[0].x, vertices[0].z);
        uvs[1] = new Vector2(vertices[1].x, vertices[1].z);

        //Add to end of array
        vertices[1 + stepCount] = rightMostVector;

        for (int i = 0; i < stepCount; i++)
        {
            float angle = -viewAngle / 2 + stepAngle * i;
            
            //Don't override last vertex value
            if (i != stepCount - 1)
            {
                //Add new vertex and uvs
                vertices[i + 2] = GetVectorFromAngle(angle) * viewRadius;
                uvs[i + 2] = new Vector2(vertices[i + 2].x, vertices[i + 2].z);
            }
            
            //Add transform position as first vertex, then prev vertex, then new vertex
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        
    }

    public void FindPlayer()
    {
        //Get colliders within viewRadius of Enemy
        Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, viewRadius);
        for (int i = 0; i < collidersInRadius.Length; i++)
        {   
            if (collidersInRadius[i].name != "Player")
            {
                continue;
            }
            Collider collider = collidersInRadius[i];
            // Find direction vector from player to collider and normalize to ease calculation 
            Vector3 colliderDirection = (collider.transform.position - transform.position).normalized;
            float colliderAngleFromPlayer = Vector3.Angle(transform.forward, colliderDirection);

            //If collider in fov
            if (colliderAngleFromPlayer < viewAngle / 2)
            {
                PlayerHandler player = collider.GetComponent<PlayerHandler>();
                //If player not hiding
                if (!player.isHiding)
                {
                    player.setIsSpotted(true);
                    //Debug.Log("Player spotted! ");
                    //GetComponentInParent<EnemyController>().setPlayerSpotted(true);
                    //FindObjectOfType<GameManager>().LoseGame();
                }
            }


        }
    }

    public Vector3 GetVectorFromAngle(float angleDegrees)
    {
        //Adds y axis angle rotation so field of view is fixed when character rotates
        angleDegrees += transform.eulerAngles.y;

        //Vector calculated using trigonometry
        return new Vector3( Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad) );  
    }

    


}
