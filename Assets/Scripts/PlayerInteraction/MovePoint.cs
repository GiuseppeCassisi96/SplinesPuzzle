using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    Transform _tr;
    Renderer _renderer;
    GameManager _gameManager;
    bool _isInsideTheInterval;
    PointInfo _pointInfo;
    int pointID;

    [SerializeField]
    float speed = 4.0f;

    private void Awake()
    {
        pointID = gameObject.GetInstanceID();
        _tr = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
                _renderer.material.color = Color.red;
            }
            else
            {
                _renderer.material.color = Color.green;
            }
        }
    }

    public void InteractionExit(int triggerID)
    {
        if (pointID == triggerID)
        {
            if (!_isInsideTheInterval)
            {
                _renderer.material.color = Color.white;
            }
            else
            {
                _renderer.material.color = Color.green;
            }
        }
    }

    public void InteractionDrag()
    {
        if(Input.GetMouseButton(0))
        {
            if (!_gameManager.ControlPointEval(_tr, _pointInfo.desiredPosition, _gameManager.tollerance))
            {
                _renderer.material.color = Color.red;
                _tr.Translate((Vector3.right * _gameManager.xAxeMouse * speed +
                Vector3.up * _gameManager.yAxeMouse * speed) * Time.deltaTime * _gameManager.pointMovementSpeed);
                _gameManager.isInteractionWithCurve = true;
                _pointInfo.isMoving = true;
            }
            else
            {
                if (!_isInsideTheInterval)
                {
                    _gameManager.isInteractionWithCurve = false;
                    _pointInfo.isMoving = false;
                    _renderer.material.color = Color.green;
                    _isInsideTheInterval = true;
                    _gameManager.AddingPoint();
                }
            }
        }
        else
        {
            _gameManager.isInteractionWithCurve = false;
            _pointInfo.isMoving = false;
        }
        
    }
}
