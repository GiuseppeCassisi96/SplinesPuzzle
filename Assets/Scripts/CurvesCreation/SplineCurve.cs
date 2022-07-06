using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineCurve : MonoBehaviour
{
    #region SerializeField var
    [SerializeField]
    int splineOrder = 4;
    [SerializeField]
    List<Transform> controlsPoint = new List<Transform>();
    [SerializeField]
    float quantization = 0.1f;
    #endregion

    #region private var
    List<float> _tParam = new List<float>();
    int _pointNumber;
    int weightSum;
    List<float> _knots = new List<float>();
    LineRenderer _line;
    Vector3[] _positions;
    #endregion

    private void Awake()
    {
        _pointNumber = controlsPoint.Count;
        UniformParameterization();
        KnotsFill();
        _line = GetComponent<LineRenderer>();
        _positions = new Vector3[_tParam.Count];
        _line.positionCount = _positions.Length;
        DrawSpline();
    }

    private void Update()
    {
        
    }

    void UniformParameterization()
    {
        for(float i = 0.0f; i <= 1; i = i + quantization)
        {
            _tParam.Add(i);
        }
    }

    void KnotsFill()
    {
        //Impongo molteplicità massima nei nodi agli estremi per far passare la spline nel primo punto
        for(int i = 1; i <= splineOrder; i++)
        {
            _knots.Add(0);
        }

        int j = 3;
        for(int i = splineOrder + 1;  i <= splineOrder + weightSum; i++)
        {
            _knots.Add(_tParam[j]);
            j++;
        }

        for(int i = splineOrder + weightSum + 1; i <= (weightSum + 2) * splineOrder; i++)
        {
            _knots.Add(1);
        }
    }

    int Bisection(float z)
    {
        var l = splineOrder;
        var u = splineOrder + weightSum + 1;
        while ((u - l) > 1)
        {
            var mid = (l + u) / 2;
            if (z < _knots[mid])
                u = mid;
            else
                l = mid;
        }
        return l;
    }


    Vector3 DeBoor(float t)
    {
        int l;
        float[] cx = new float[splineOrder + 1];
        float[] cy = new float[splineOrder + 1];
        l = Bisection(t);
        for(int i = 1; i <= splineOrder; i++)
        {
            cx[i] = controlsPoint[i + l - splineOrder - 1].position.x;
            cy[i] = controlsPoint[i + l - splineOrder - 1].position.y;
        }

        for (int j = 2; j <= splineOrder; j++)
        {
            for (int i = splineOrder; i >= j; i--)
            {
                float denom = _knots[i + l - j + 1] - _knots[i + l - splineOrder];
                float  a1 = (t - _knots[i + l - splineOrder]) / denom;
                float a2 = 1 - a1; 
                cx[i] = a1 * cx[i] + a2 * cx[i - 1];
                cy[i] = a1 * cy[i] + a2 * cy[i - 1];
            }
        }

        var res = new Vector3(cx[splineOrder], cy[splineOrder], 0);
        return res;
    }

    void DrawSpline()
    {
        Vector3 tempVector = Vector3.zero;
        for (int i = 0; i < _tParam.Count; i++)
        {
            tempVector = Vector3.zero;
            tempVector = DeBoor(_tParam[i]);
            _positions[i] = tempVector; 
        }

        _line.SetPositions(_positions);
    }
}
