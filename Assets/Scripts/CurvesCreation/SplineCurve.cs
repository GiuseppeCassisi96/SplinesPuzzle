using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WrapperList
{
    public List<Transform> pointsOfBezierCurve;
}

public class SplineCurve : MonoBehaviour
{
    #region private var
    LineRenderer _lineRenderer;
    const int NUM_CURVES = 2;
    Vector3[] _positions;
    GameManager _gameManager;
    int _resolutionOfCurve;
    List<float> _nodesVector;
    #endregion

    #region serialize field var
    [SerializeField]
    List<WrapperList> bezierCurves;
    #endregion

    #region public var
    public List<Transform> Points;
    #endregion

    #region Properties
    public List<float> NodesVector
    {
        get
        {
            return _nodesVector;
        }
    }
    #endregion

    void FillNodesVector()
    {
        _nodesVector = new List<float>();
        _nodesVector.Add(0);
        for(int i = 1; i <= NUM_CURVES; i++)
        {
            float tempValue = _nodesVector[i - 1] + UnityEngine.Random.Range(0.01f, 0.99f);
            _nodesVector.Add(tempValue);
        }
    }

    float SimpleRapport(int index)
    {
        int begin = index - 1;
        int end = index + 1;
        /*{A, C, B} = 1/c 
         * 
         * (B - A)
         * -------
         * (C - A)
         */
        return (_nodesVector[index] - _nodesVector[begin]) / (_nodesVector[end] - _nodesVector[begin]);
    }

    void PlaceJunctionPoint(int index, float interpolationValue)
    {
        Points[index].position = Vector3.Lerp(Points[index - 1].position, Points[index + 1].position, interpolationValue);
    }

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _resolutionOfCurve = _gameManager.CurveResolution;
        _lineRenderer = GetComponent<LineRenderer>();
        FillNodesVector();
        int nodeIndex = 1;
        int pointIndex = 2;
        while(true)
        {
            if (nodeIndex - 1 < 0 || nodeIndex + 1 > _nodesVector.Count - 1)
            {
                break;
            }
            if((pointIndex % 2 == 0) && (pointIndex != 0 && pointIndex != Points.Count - 1))
            {
                break;
            }
            float interpolationValue = SimpleRapport(nodeIndex);
            Debug.Log(interpolationValue);
            PlaceJunctionPoint(pointIndex, interpolationValue);
            nodeIndex++;
            pointIndex = pointIndex + 2;
        }
        _positions = new Vector3[_resolutionOfCurve];
        _lineRenderer.positionCount = _resolutionOfCurve * NUM_CURVES;
        DrawQuadratic();

    }

    private void Update()
    {
        if (_gameManager.isInteractionWithCurve)
            DrawQuadratic();
    }

    void DrawQuadratic()
    {
        for (int j = 0; j < bezierCurves.Count; j++)
        {
            float t = 0.0f;
            for (int i = 0; i < _resolutionOfCurve; i++)
            {
                t = i / (float)_resolutionOfCurve;
                _positions[i] = QuadraticBezierCurve(t, 
                    bezierCurves[j].pointsOfBezierCurve[0].position,
                    bezierCurves[j].pointsOfBezierCurve[1].position,
                    bezierCurves[j].pointsOfBezierCurve[2].position);
                _lineRenderer.SetPosition((j * _resolutionOfCurve) + i, _positions[i]);
            }
            
        }
    }

    public Vector3 QuadraticBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (Mathf.Pow(1 - t, 2) * p0) + (2 * (1 - t) * t * p1) + (Mathf.Pow(t, 2) * p2);
    }

}
