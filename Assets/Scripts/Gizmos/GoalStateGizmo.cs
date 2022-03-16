using UnityEngine;
using UnityEditor;

public class GoalStateGizmo : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3(3, 1, 3));
    }
}
