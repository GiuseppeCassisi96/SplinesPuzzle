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

    
    LevelEvaluation levelEvaluation;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        _newPointPosition = Vector2.zero;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        offsetEval = _gameManager.tollerance;
        levelEvaluation = GetComponent<LevelEvaluation>();
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
            _gameManager.pointIsMoving = false;
        }
        else
        {
            _renderer.material.color = Color.green;
            _gameManager.pointIsMoving = false;
        }
    }

    private void OnMouseDrag()
    {        

        if(!levelEvaluation.ControlPointEval())
        {
            _renderer.material.color = Color.red;
            _tr.Translate((Vector3.right * _gameManager.xAxeMouse +
            Vector3.up * _gameManager.yAxeMouse) * Time.deltaTime * _gameManager.pointMovementSpeed);
            _gameManager.pointIsMoving = true;
        }
        else
        {
            if(!isInsideTheInterval)
            {
                _gameManager.pointIsMoving = false;
                _renderer.material.color = Color.green;
                isInsideTheInterval = true;
                _gameManager.AddingPoint();
            }
            
        }

        
    }





}
