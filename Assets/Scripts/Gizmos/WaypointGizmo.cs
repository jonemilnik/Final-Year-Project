using UnityEngine;
using UnityEditor;

public class WaypointGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        GameObject gameObject = Selection.activeGameObject;

        if (gameObject == transform.parent.gameObject)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, Vector3.one);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
