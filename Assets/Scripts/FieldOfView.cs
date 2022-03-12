using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public Vector3 getVectorFromAngle(float angleDegrees)
    {
        //Adds y axis angle rotation so field of view is fixed when character rotates
        angleDegrees += transform.eulerAngles.y;

        //Vector calculated using trigonometry
        return new Vector3( Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad) );  
    }


}
