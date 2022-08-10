using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInfo : MonoBehaviour
{
    #region private var
    bool isMoving;
    #endregion


    #region SerializeField var
    [SerializeField]
    Transform pointTransform;
    [SerializeField]
    Vector3 desiredPosition;
    [SerializeField]
    bool isJunctionPoint;
    [SerializeField]
    PointInfo otherJunctionPoint;
    #endregion


    #region Properties
    public Transform PointTransform
    {
        get { return pointTransform; }

    }

    public Vector3 DesiredPosition
    {
        get { return desiredPosition; }
    }

    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    public bool IsJunctionPoint
    {
        get { return isJunctionPoint; }
    }
    #endregion

    #region unity methods
    private void Start()
    {
        if(isJunctionPoint)
        desiredPosition = otherJunctionPoint.pointTransform.localPosition;
    }
    #endregion
}
