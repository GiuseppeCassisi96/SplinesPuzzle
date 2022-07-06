using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvaluation : MonoBehaviour
{
    GameManager _gameManager;
    float _offsetEval;
    Transform _tr;

    [SerializeField]
    SplinesCreation CurveB, CurveA;

   public Vector3 desiredPosition;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _offsetEval = _gameManager.tollerance;
        _tr = GetComponent<Transform>();
    }
    public bool ControlPointEval()
    {
        bool xSide = (_tr.localPosition.x >= (desiredPosition.x - _offsetEval)) &&
           (_tr.localPosition.x <= (desiredPosition.x + _offsetEval));
        bool ySide = (_tr.localPosition.y >= (desiredPosition.y - _offsetEval)) &&
            (_tr.localPosition.y <= (desiredPosition.y + _offsetEval));
        return xSide && ySide;
    }

    public bool ControlKnotsValue()
    {
        for(int i = 0; i < CurveB.knots.nodes.Count; i++)
        {
            if(CurveA.knots.nodes[i] != CurveB.knots.nodes[i])
            {
                return false;
            }
        }
        Debug.Log("PIPPOOO");
        _gameManager.AddingPoint();
        return true;
    }
}
