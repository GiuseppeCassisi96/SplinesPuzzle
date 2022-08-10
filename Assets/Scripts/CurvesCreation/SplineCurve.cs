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
    [SerializeField]
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
    #region user define methods

    /// <summary>
    /// Fills the nodes vector, every node's value is composed by adding the value 
    /// of the previous node with a random offset 
    /// </summary>
    void FillNodesVector()
    {
        _nodesVector = new List<float>();
        _nodesVector.Add(UnityEngine.Random.Range(0.01f, 0.99f));
        for(int i = 1; i <= num_curves; i++)
        {
            float tempValue = _nodesVector[i - 1] + UnityEngine.Random.Range(0.01f, 0.99f);
            _nodesVector.Add(tempValue);
        }

        Debug.Log("U0: " + _nodesVector[0] + " U1: " + _nodesVector[1] + " U2: " + _nodesVector[2]);
    }

    /// <summary>
    /// Does the simple rapport between three nodes 
    /// </summary>
    /// <param name="index"> Is the index of the node which we want know the simple rapport </param>
    /// <returns> return an interpolation value between 0 and 1 </returns>
    float SimpleRapport(int index)
    {
        int begin = index - 1;
        int end = index + 1;
        /*{A, B, C} = interpolation value between 0 and 1 
         * 
         * (B - A)
         * ------- = 1/c
         * (C - A)
         */
        return (_nodesVector[index] - _nodesVector[begin]) / (_nodesVector[end] - _nodesVector[begin]);
    }

    /// <summary>
    /// Places the junction point between the previous  one and the next one 
    /// based on the interpolation value
    /// </summary>
    /// <param name="index"> Is the index of junction point </param>
    /// <param name="interpolationValue"> Is the interpolation value used to place the 
    /// junction point </param>
    void PlaceJunctionPoint(int index, float interpolationValue)
    {
        Points[index].PointTransform.position = Vector3.Lerp(Points[index - 1].PointTransform.position, 
            Points[index + 1].PointTransform.position, interpolationValue);
       bool isInside = _gameManager.GMControlPointEval(Points[index].PointTransform, 
           Points[index].DesiredPosition, _gameManager.Tollerance);
        if (isInside)
            _gameManager.AddingPoint();
    }

    /// <summary>
    /// Changes the value of a node 
    /// </summary>
    /// <param name="index"> Is the index of node that we want change </param>
    /// <param name="value"> Is the new value for the node </param>
    /// <exception cref="IndexNotValid" cref="ValueNotValid"> Can we have an 'IndexNotValid' and a 
    /// 'ValueNotValid' exception </exception>
    public void ChangeKnotsValue(int index, float value)
    {
        if (index  < 0 || index > _nodesVector.Count - 1)
        {
            throw new IndexNotValid("Index not valid");

        }

        if (index == NodesVector.Count - 1)
        {
            if(value <= NodesVector[index-1])
            {
                throw new ValueNotValid("Value not valid");
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
            if (value >= NodesVector[1])
            {
                throw new ValueNotValid("Value not valid");
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
                throw new ValueNotValid("Value not valid");
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

    /// <summary>
    /// Updates the position of junction point
    /// </summary>
    public void ReplaceJunction()
    {
        int nodeIndex = 1;
        int pointIndex = 2;
        while (true)
        {
            if (nodeIndex + 1 > _nodesVector.Count - 1)
            {
                break;

            }
            if (pointIndex == Points.Count - 1)
            {
                break;

            }
            float interpolationValue = SimpleRapport(nodeIndex);
            PlaceJunctionPoint(pointIndex, interpolationValue);
            nodeIndex++;
            pointIndex = pointIndex + 2;
        }
    }
    /// <summary>
    /// Draws the Spline 
    /// </summary>
    public void DrawSpline()
    {
        for (int j = 0; j < bezierCurves.Count; j++)
        {
            float t = 0.0f;
            for (int i = 0; i < _resolutionOfCurve; i++)
            {
                t = i / (float)_resolutionOfCurve;
                _positions[i] = QuadraticBezierCurve(t,
                    bezierCurves[j].pointsOfBezierCurve[0].PointTransform.position,
                    bezierCurves[j].pointsOfBezierCurve[1].PointTransform.position,
                    bezierCurves[j].pointsOfBezierCurve[2].PointTransform.position);
                _lineRenderer.SetPosition((j * _resolutionOfCurve) + i, _positions[i]);
            }

        }
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
        return (Mathf.Pow(1 - t, 2) * p0) + (2 * (1 - t) * t * p1) + (Mathf.Pow(t, 2) * p2);
    }

    #endregion

    #region unity methods
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
            if (nodeIndex + 1 > _nodesVector.Count - 1)
            {
                break;
                
            }
            if(pointIndex == Points.Count - 1)
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
        DrawSpline();

    }

    private void Update()
    {
        if (_gameManager.isInteractionWithCurve)
        {
            DrawSpline();
            Debug.Log("DrawSpline");
        }
            
    }
    #endregion


}
