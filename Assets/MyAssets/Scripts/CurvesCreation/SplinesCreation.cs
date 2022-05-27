using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SplinesCreation : MonoBehaviour
{
    #region private var
    LineRenderer _line;
    List<Vector3> positions = new List<Vector3>();
    Vector3 tempVector;
    int _intervalLenght;
    #endregion

    #region SerializeField var
    [SerializeField]
    List<Transform> controlPoints = new List<Transform>();
    [SerializeField]
    int _gradeSpline = 3;
    #endregion

    #region public var
    [HideInInspector]
    public List<float> _knotsVector = new List<float>();
    #endregion 

    #region Unity methods
    private void Awake()
    {
        int knotsNum = controlPoints.Count + _gradeSpline + 1;
        int n = 0;
        //Il primo e l'ultimo punto hanno molteplicità 4 con lo scopo di far passare la curva esattamente per 
        //il primo punto di controllo e l'ultimo 
        for (int i = 0; i < knotsNum; i++)
        {
            if (i <= _gradeSpline) //Estremo sinistro vettore nodi
            {
                _knotsVector.Add(0);
            }
            else if (i >= knotsNum - (_gradeSpline + 1)) //Estremo destro vettore nodi
            {
                _knotsVector.Add(n + 1);
            }
            else
            {
                _knotsVector.Add(i);
                n = i;
            }
            Debug.Log("NODE: " + _knotsVector[i]);
        }
        int index = 0;
        for (float t = _knotsVector[0]; t < _knotsVector[_knotsVector.Count - 1]; t += 0.2f)
        {
            index++;
        }
        _intervalLenght = index;
        _line = GetComponent<LineRenderer>();
        _line.positionCount = _intervalLenght;
        tempVector = Vector3.zero;

    }

    private void Update()
    {
        CreateCurve();
    }

    #endregion

    #region User define methods

    float DeBoor(float t, int i, int k)
    {
        if (k == 0)
        {
            if (Knot(i) <= t && t < Knot(i + 1))
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }
        else
        {
            float result = 0.0f;
            if (Knot(i + k) - Knot(i) != 0)
            {
                result += ((t - Knot(i)) / (Knot(i + k) - Knot(i))) * DeBoor(t, i, k - 1);
            }
            if (Knot(i + k + 1) - Knot(i + 1) != 0)
            {
                result += ((Knot(i + k + 1) - t) / (Knot(i + k + 1) - Knot(i + 1))) * DeBoor(t, i + 1, k - 1);
            }
            return result;
        }
    }

    void CreateCurve()
    {
        if (controlPoints.Count >= _gradeSpline)
        {
            for (float t = _knotsVector[0]; t < _knotsVector[_knotsVector.Count - 1]; t += 0.2f)
            {
                tempVector = Vector3.zero;
                for (int i = 0; i < controlPoints.Count; i++)
                {
                    float weightForControl = DeBoor(t, i, _gradeSpline);
                    tempVector += weightForControl * controlPoints[i].position;
                }
                positions.Add(tempVector);
            }
            _line.SetPositions(positions.ToArray());
            positions.Clear();
        }
    }

    float Knot(int index)
    {
        return _knotsVector[index];
    }
    #endregion

}
