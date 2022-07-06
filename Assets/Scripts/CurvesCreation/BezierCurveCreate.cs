using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveCreate : MonoBehaviour
{
    LineRenderer _line;
    int _curveResolution;
    [HideInInspector]
    public Vector3[] positions;
    [SerializeField]
    Transform[] points;
    
    

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _curveResolution = GameObject.Find("GameManager").GetComponent<GameManager>().CURVE_RESOLUTION;
        positions = new Vector3[_curveResolution];
        _line.positionCount = _curveResolution;
             
    }

    private void Update()
    {
        DrawLine();
    }

    public Vector3 QuadraticBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (Mathf.Pow(1 - t, 2) * p0) + (2 * (1-t)*t*p1) + (Mathf.Pow(t,2) * p2);
    }

    Vector3 CubicBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (Mathf.Pow(1 - t, 3) * p0) + (3 * t * Mathf.Pow(1 - t, 2) * p1)
            + (3 * Mathf.Pow(t, 2) * (1 - t) * p2) + Mathf.Pow(t, 3) * p3;
    }

    void DrawLine()
    {
        float t = 0.0f;
        for(int i = 1; i <= _curveResolution; i++)
        {
            t = i / (float)_curveResolution;
            positions[i - 1] = QuadraticBezierCurve(t, points[0].position, points[1].position, points[2].position);
        }
        _line.SetPositions(positions);
    }
}
