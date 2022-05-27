using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeKnotsValue : MonoBehaviour
{

    List<float> _knotsVector = new List<float>();

    void SetKnots(List<float> knots)
    {
        _knotsVector = knots;
    }
}
