using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStateGizmo : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(3, 1, 3));
    }
}
