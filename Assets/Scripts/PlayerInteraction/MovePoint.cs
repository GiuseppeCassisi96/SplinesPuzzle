using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    Transform _tr;
    Ray _ray;
    Renderer _renderer;
    Vector2 _newPointPosition;
    GameManager _gameManager;
    float offsetEval;
    bool isInsideTheInterval;

    
    PointInfo pointInfo;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        _newPointPosition = Vector2.zero;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        offsetEval = _gameManager.tollerance;
        pointInfo = GetComponent<PointInfo>();
    }

    private void OnMouseEnter()
    {
        if(!isInsideTheInterval)
        {
            _renderer.material.color = Color.red;
        }
        else
        {
            _renderer.material.color = Color.green;
        }
        
    }

    private void OnMouseExit()
    {
        if (!isInsideTheInterval)
        {
            _renderer.material.color = Color.white;
        }
        else
        {
            _renderer.material.color = Color.green;
        }
    }

    private void OnMouseUp()
    {
        if (!isInsideTheInterval)
        {
            _renderer.material.color = Color.white;
            _gameManager.isInteractionWithCurve = false;
            pointInfo.isMoving = false;
        }
        else
        {
            _renderer.material.color = Color.green;
            _gameManager.isInteractionWithCurve = false;
            pointInfo.isMoving = false;
        }
    }

    private void OnMouseDrag()
    {        

        if(!_gameManager.ControlPointEval(_tr, pointInfo.desiredPosition, _gameManager.tollerance))
        {
            _renderer.material.color = Color.red;
            _tr.Translate((Vector3.right * _gameManager.xAxeMouse +
            Vector3.up * _gameManager.yAxeMouse) * Time.deltaTime * _gameManager.pointMovementSpeed);
            _gameManager.isInteractionWithCurve = true;
            pointInfo.isMoving = true;
        }
        else
        {
            if(!isInsideTheInterval)
            {
                _gameManager.isInteractionWithCurve = false;
                pointInfo.isMoving = false;
                _renderer.material.color = Color.green;
                isInsideTheInterval = true;
                _gameManager.AddingPoint();
            }
            
        }

        
    }





}
