using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WrapperList
{
    public List<PointInfo> pointsOfBezierCurve;
}

public class SplineCurve : MonoBehaviour
{
    #region private var
    LineRenderer _lineRenderer;
    
    Vector3[] _positions;
    GameManager _gameManager;
    int _resolutionOfCurve;
    List<float> _nodesVector;
    Dictionary<int, int> _indexNodeToIndexPoint;
    #endregion

    #region serialize field var
    [SerializeField]
    List<WrapperList> bezierCurves;
    [SerializeField]
    int num_curves = 2;
    #endregion

    #region public var
    public List<PointInfo> Points;
    #endregion

    #region Properties
    public List<float> NodesVector
    {
        get
        {
            return _nodesVector;
        }
    }

    public Dictionary<int, int> IndexNodeToIndexPoint
    {
        get
        {
            return _indexNodeToIndexPoint;
        }
    }
    #endregion

    void FillNodesVector()
    {
        _nodesVector = new List<float>();
        _nodesVector.Add(0);
        for(int i = 1; i <= num_curves; i++)
        {
            float tempValue = _nodesVector[i - 1] + UnityEngine.Random.Range(0.01f, 0.99f);
            _nodesVector.Add(tempValue);
        }

        Debug.Log("U0: " + _nodesVector[0] + " U1: " + _nodesVector[1] + " U2: " + _nodesVector[2]);
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
        Points[index].pointTransform.position = Vector3.Lerp(Points[index - 1].pointTransform.position, 
            Points[index + 1].pointTransform.position, interpolationValue);
       bool isInside = _gameManager.ControlPointEval(Points[index].pointTransform, 
           Points[index].desiredPosition, _gameManager.Tollerance);
        if (isInside)
            _gameManager.AddingPoint();
    }

    public void ChangeKnotsValue(int index, float value)
    {
        if (index  < 0 || index > _nodesVector.Count - 1)
        {
            throw new Exception("Index not valid");

        }
        if (index == NodesVector.Count - 1)
        {
            if(value <= NodesVector[index-1])
            {
                throw new Exception("Value not valid");
            }
            else
            {
                NodesVector[index] = value;
                int nodeIndex = index - 1;
                int pointIndex = IndexNodeToIndexPoint[nodeIndex];
                float interpolationValue = SimpleRapport(nodeIndex);
                PlaceJunctionPoint(pointIndex, interpolationValue);
            }
        }
        else if(index == 0)
        {
            if (value >= NodesVector[0])
            {
                throw new Exception("Value not valid");
            }
            else
            {
                NodesVector[index] = value;
                int nodeIndex = index + 1;
                int pointIndex = IndexNodeToIndexPoint[nodeIndex];
                float interpolationValue = SimpleRapport(nodeIndex);
                PlaceJunctionPoint(pointIndex, interpolationValue);
            }
        }
        else
        {
            if(value <= NodesVector[index-1] || value >= NodesVector[index + 1])
            {
                throw new Exception("Value not valid");
            }
            else
            {
                NodesVector[index] = value;
                int nodeIndex = index;
                int pointIndex = IndexNodeToIndexPoint[nodeIndex];
                float interpolationValue = SimpleRapport(nodeIndex);
                PlaceJunctionPoint(pointIndex, interpolationValue);
            }
        }
    }

    public void ReplaceJunction()
    {
        int nodeIndex = 1;
        int pointIndex = 2;
        while (true)
        {
            if (nodeIndex - 1 < 0 || nodeIndex + 1 > _nodesVector.Count - 1)
            {
                break;

            }
            if ((pointIndex % 2 != 0) && (pointIndex == 0 && pointIndex == Points.Count - 1))
            {
                break;

            }
            float interpolationValue = SimpleRapport(nodeIndex);
            PlaceJunctionPoint(pointIndex, interpolationValue);
            nodeIndex++;
            pointIndex = pointIndex + 2;
        }
    }

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _resolutionOfCurve = _gameManager.CurveResolution;
        _lineRenderer = GetComponent<LineRenderer>();
        _indexNodeToIndexPoint = new Dictionary<int, int>();
        FillNodesVector();
        int nodeIndex = 1;
        int pointIndex = 2;
        while(true)
        {
            if (nodeIndex - 1 < 0 || nodeIndex + 1 > _nodesVector.Count - 1)
            {
                break;
                
            }
            if((pointIndex % 2 != 0) && (pointIndex == 0 && pointIndex == Points.Count - 1))
            {
                break;
                
            }
            float interpolationValue = SimpleRapport(nodeIndex);
            PlaceJunctionPoint(pointIndex, interpolationValue);
            _indexNodeToIndexPoint.Add(nodeIndex, pointIndex);
            
            nodeIndex++;
            pointIndex = pointIndex + 2;
        }

        _positions = new Vector3[_resolutionOfCurve];
        _lineRenderer.positionCount = _resolutionOfCurve * num_curves;
        DrawQuadratic();

    }

    private void Update()
    {
        if (_gameManager.isInteractionWithCurve)
        {
            DrawQuadratic();
            Debug.Log("DrawSpline");
        }
            
    }

    public void DrawQuadratic()
    {
        for (int j = 0; j < bezierCurves.Count; j++)
        {
            float t = 0.0f;
            for (int i = 0; i < _resolutionOfCurve; i++)
            {
                t = i / (float)_resolutionOfCurve;
                _positions[i] = QuadraticBezierCurve(t, 
                    bezierCurves[j].pointsOfBezierCurve[0].pointTransform.position,
                    bezierCurves[j].pointsOfBezierCurve[1].pointTransform.position,
                    bezierCurves[j].pointsOfBezierCurve[2].pointTransform.position);
                _lineRenderer.SetPosition((j * _resolutionOfCurve) + i, _positions[i]);
            }
            
        }
    }

    public Vector3 QuadraticBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (Mathf.Pow(1 - t, 2) * p0) + (2 * (1 - t) * t * p1) + (Mathf.Pow(t, 2) * p2);
    }

}
