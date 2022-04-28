using UnityEngine;
using UnityEditor;

public class WaypointGizmo : MonoBehaviour
{
    
    private void OnDrawGizmosSelected()
    {
        if (string.Equals(transform.parent.tag, "PlayerWaypoints"))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position, Vector3.one);
        } else if (string.Equals(transform.parent.tag, "EnemyWaypoints"))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
        
    }
}
