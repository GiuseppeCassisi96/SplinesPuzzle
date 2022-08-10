using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    Transform _tr;
    Renderer _renderer;
    GameManager _gameManager;
    ColorManager _colorManager;
    bool _isInsideTheInterval;
    PointInfo _pointInfo;
    int pointID;
    ChangePointColor _pointColor;



    private void Awake()
    {
        pointID = gameObject.GetInstanceID();
        _tr = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        _pointColor = GetComponent<ChangePointColor>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _colorManager = GameObject.Find("ColorManager").GetComponent<ColorManager>();
        _pointInfo = GetComponent<PointInfo>();
    }

    private void OnEnable()
    {
        EventManager.EnterEvent += InteractionEnter;
        EventManager.ExitEvent += InteractionExit;
    }
    private void OnDisable()
    {
        EventManager.EnterEvent -= InteractionEnter;
        EventManager.ExitEvent -= InteractionExit;
    }

    public void InteractionEnter(int triggerID)
    {
        if (pointID == triggerID)
        {
            if (!_isInsideTheInterval)
            {
                InteractionDrag();
                _colorManager.SetColor(_renderer, _pointColor.PointColor);

            }
            else
            {
                _colorManager.SetColor(_renderer, Color.green);
            }
        }
    }

    public void InteractionExit(int triggerID)
    {
        if (pointID == triggerID)
        {
            PlayerMove.IsLook = false;
            if (!_isInsideTheInterval)
            {
                _colorManager.SetColor(_renderer, Color.white);
            }
            else
            {
                _colorManager.SetColor(_renderer, Color.green);
            }
        }
    }

    public void InteractionDrag()
    {
        if(Input.GetMouseButton(0))
        {
            if (!_gameManager.GMControlPointEval(_tr, _pointInfo.DesiredPosition, _gameManager.Tollerance))
            {
                _tr.Translate((Vector3.right * _gameManager.xAxeMouse +
                Vector3.up * _gameManager.yAxeMouse) * Time.deltaTime * _gameManager.pointMovementSpeed);
                _gameManager.isInteractionWithCurve = true;
                _pointInfo.IsMoving = true;
                PlayerMove.IsLook = true;
                EventManager.LookAction(_tr);
            }
            else
            {
                PlayerMove.IsLook = false;
                _gameManager.isInteractionWithCurve = false;
                _pointInfo.IsMoving = false;
                _colorManager.SetColor(_renderer, Color.green);
                _isInsideTheInterval = true;
                _gameManager.AddingPoint();
            }
        }
        else
        {
            PlayerMove.IsLook = false;
            _gameManager.isInteractionWithCurve = false;
            _pointInfo.IsMoving = false;
        }
        
    }
}
