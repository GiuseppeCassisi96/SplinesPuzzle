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

    [SerializeField]
    float speed = 4.0f;


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
                _colorManager.SetColor(_renderer, _pointColor.pointColor);

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
            PlayerMove._isLook = false;
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
            if (!_gameManager.ControlPointEval(_tr, _pointInfo.desiredPosition, _gameManager.tollerance))
            {
                _tr.Translate((Vector3.right * _gameManager.xAxeMouse * speed +
                Vector3.up * _gameManager.yAxeMouse * speed) * Time.deltaTime * _gameManager.pointMovementSpeed);
                _gameManager.isInteractionWithCurve = true;
                _pointInfo.isMoving = true;
                PlayerMove._isLook = true;
                EventManager.LookAction(_tr);
                Debug.Log("Move");
            }
            else
            {
                PlayerMove._isLook = false;
                _gameManager.isInteractionWithCurve = false;
                _pointInfo.isMoving = false;
                _colorManager.SetColor(_renderer, Color.green);
                _isInsideTheInterval = true;
                _gameManager.AddingPoint();
            }
        }
        else
        {
            PlayerMove._isLook = false;
            _gameManager.isInteractionWithCurve = false;
            _pointInfo.isMoving = false;
        }
        
    }
}
