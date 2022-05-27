using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public static event Action addingPoint;

    Transform _tr;
    Ray _ray;
    Renderer _renderer;
    Vector2 _newPointPosition;
    GameManager _gameManager;
    float offsetEval;
    bool isInsideTheInterval;

    [SerializeField]
    Vector2 desiredPosition;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        _newPointPosition = Vector2.zero;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        offsetEval = _gameManager.tollerance;
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
        }
        else
        {
            _renderer.material.color = Color.green;
        }
    }

    private void OnMouseDrag()
    {        

        if(!curveEval())
        {
            _renderer.material.color = Color.red;
            _tr.Translate((Vector3.right * _gameManager.xAxeMouse +
            Vector3.up * _gameManager.yAxeMouse) * Time.deltaTime * _gameManager.pointMovementSpeed);
        }
        else
        {
            if(!isInsideTheInterval)
            {
                _renderer.material.color = Color.green;
                isInsideTheInterval = true;
                addingPoint.Invoke();
            }
            
        }

        
    }

    bool curveEval()
    {
        bool xSide = (_tr.localPosition.x >= (desiredPosition.x - offsetEval)) && 
            (_tr.localPosition.x <= (desiredPosition.x + offsetEval));
        bool ySide = (_tr.localPosition.y >= (desiredPosition.y - offsetEval)) && 
            (_tr.localPosition.y <= (desiredPosition.y + offsetEval));
        return xSide && ySide;
    }





}
