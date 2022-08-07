using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInfo : MonoBehaviour
{
    [HideInInspector]
    public SplineCurve mySpline;
    public Transform pointTransform;
    public Vector3 desiredPosition;
    [HideInInspector]
    public bool isMoving;
    public bool isJunctionPoint;
    public PointInfo otherJunctionPoint;


    private void Start()
    {
        mySpline = transform.parent.gameObject.GetComponent<SplineCurve>();
        if(isJunctionPoint)
        desiredPosition = otherJunctionPoint.pointTransform.localPosition;
    }

}
