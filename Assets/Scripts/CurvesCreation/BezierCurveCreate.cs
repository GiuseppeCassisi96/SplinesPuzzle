using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveCreate : MonoBehaviour
{
    enum BezierType
    {
        Quadratic, 
        Cubic
    }

    LineRenderer _line;
    int _curveResolution;
    [HideInInspector]
    public Vector3[] positions;
    [SerializeField]
    Transform[] points;
    [SerializeField]
    BezierType bezierType;
    delegate void DrawFunction();
    DrawFunction drawFunction;
    
    

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _curveResolution = GameObject.Find("GameManager").GetComponent<GameManager>().CurveResolution;
        positions = new Vector3[_curveResolution];
        _line.positionCount = _curveResolution;
        if(bezierType == BezierType.Cubic)
        {
            drawFunction = DrawCubic;
        }
        else
        {
            drawFunction = DrawQuadratic;
        }
             
    }

    private void Update()
    {
        drawFunction.Invoke();
    }
    /// <summary>
    /// Computes the quadratic Bezier curve using De Casteljau's algorithm
    /// </summary>
    /// <param name="t"> The parameter used to evaluate the curve </param>
    /// <param name="p0"> First control point </param>
    /// <param name="p1"> Second contol point </param>
    /// <param name="p2"> Third control point </param>
    /// <returns> Returns a position based on 't' parameter and on his three control points </returns>
    public Vector3 QuadraticBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (Mathf.Pow(1 - t, 2) * p0) + (2 * (1-t)*t*p1) + (Mathf.Pow(t,2) * p2);
    }

    /// <summary>
    /// Computes the cubic Bezier curve using De Casteljau's algorithm
    /// </summary>
    /// <param name="t"> The parameter used to evaluate the curve </param>
    /// <param name="p0"> First control point </param>
    /// <param name="p1"> Second contol point </param>
    /// <param name="p2"> Third control point </param>
    /// <param name="p3"> Fourth control point </param>
    /// <returns> Returns a position based on 't' parameter and on his three control points </returns>
    public Vector3 CubicBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (Mathf.Pow(1 - t, 3) * p0) + (3 * t * Mathf.Pow(1 - t, 2) * p1)
            + (3 * Mathf.Pow(t, 2) * (1 - t) * p2) + Mathf.Pow(t, 3) * p3;
    }

    void DrawQuadratic()
    {
        float t = 0.0f;
        for(int i = 1; i <= _curveResolution; i++)
        {
            t = i / (float)_curveResolution;
            positions[i - 1] = QuadraticBezierCurve(t, points[0].position, points[1].position, points[2].position);
        }
        _line.SetPositions(positions);
    }

    void DrawCubic()
    {
        float t = 0.0f;
        for (int i = 1; i <= _curveResolution; i++)
        {
            t = i / (float)_curveResolution;
            positions[i - 1] = CubicBezierCurve(t, points[0].position, points[1].position, points[2].position, points[3].position);
        }
        _line.SetPositions(positions);
    }
}
