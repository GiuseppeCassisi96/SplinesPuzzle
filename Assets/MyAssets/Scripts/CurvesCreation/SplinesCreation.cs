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
    int index = 0;
    #endregion

    #region SerializeField var
    [SerializeField]
    List<Transform> controlPoints = new List<Transform>();
    [SerializeField]
    int multiplicity = 2;
    [SerializeField]
    float quantization = 0.1f;
    [SerializeField]
    GameManager gameManager;
    #endregion

    #region public var
    public List<float> knots = new List<float>();
    #endregion 

    #region Unity methods
    private void Awake()
    {
        int knotsNum = controlPoints.Count + GameManager.SPLINE_GRADE + 1;
        int n = 0;
        //Il primo e l'ultimo punto hanno molteplicitÓ 4 con lo scopo di far passare la curva esattamente per 
        //il primo punto di controllo e l'ultimo 
        for (int i = 0; i < knotsNum; i++)
        {
            if (i < multiplicity) //Estremo sinistro vettore nodi
            {
                knots.Add(0);
            }
            else if (i >= knotsNum - (multiplicity)) //Estremo destro vettore nodi
            {
                knots.Add(knotsNum);
            }
            else
            {
                knots.Add(i);
                n = i;
            }
        }
       
        for (float t = knots[0]; t < knots[knots.Count - 1]; t += quantization)
        {
            index++;
        }
        _intervalLenght = index;
        _line = GetComponent<LineRenderer>();
        tempVector = Vector3.zero;
        CreateCurve();

    }

    private void Update()
    {
        if(gameManager.pointIsMoving)
        CreateCurve();
    }

    #endregion

    #region User define methods

    float DeBoorCox(float t, int i, int k)
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
            if (Knot(i + k) - Knot(i) != 0)//Per evitare casi di indeterminazione
            {
                float first = (t - Knot(i)) / (Knot(i + k) - Knot(i)) * DeBoorCox(t, i, k - 1);
                result += first;
            }
            if (Knot(i + k + 1) - Knot(i + 1) != 0)//Per evitare casi di indeterminazione
            {
                float second = (Knot(i + k + 1) - t) / (Knot(i + k + 1) - Knot(i + 1)) * DeBoorCox(t, i + 1, k - 1);
                result += second;
            }
            return result;
        }
    }

    void CreateCurve()
    {
        for (float t = knots[0] + quantization; t <= knots[knots.Count - 1]; t += quantization)
        {
            tempVector = Vector3.zero;
            for (int i = 0; i < controlPoints.Count; i++)
            {
                tempVector += DeBoorCox(t, i, GameManager.SPLINE_GRADE) * controlPoints[i].position;
            }
            positions.Add(tempVector);
        }
        _line.positionCount = positions.Count;
        if(multiplicity < GameManager.SPLINE_GRADE + 1)
        {
            //Post processing
            for (int i = 0; i < 22; i++)
            {
                positions[i] = positions[22];
            }
            for (int i = positions.Count - 23; i < positions.Count; i++)
            {
                positions[i] = positions[positions.Count - 23];
            }
        }     
        _line.SetPositions(positions.ToArray());
        positions.Clear();
    }

    float Knot(int index)
    {
        return knots[index];
    }
    #endregion

}
