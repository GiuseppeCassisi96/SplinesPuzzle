using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChangePointColor : MonoBehaviour
{
    Renderer _renderer;
    ColorManager _colorManager;
    Transform _tr;
    PointInfo _pointInfo;

    [HideInInspector]
    Color pointColor;

    public Color PointColor
    {
        get { return pointColor; }
    }
    private void Awake()
    {
        _colorManager = GameObject.Find("ColorManager").GetComponent<ColorManager>();
        _renderer = GetComponent<Renderer>();
        _tr = GetComponent<Transform>();
        _pointInfo = GetComponent<PointInfo>();
        pointColor = Color.blue;
    }

    private void Update()
    {
        if (_pointInfo.IsMoving || _pointInfo.IsJunctionPoint)
        {
            float distance = Vector2.Distance(_tr.localPosition, _pointInfo.DesiredPosition);
            float value = Mathf.Clamp01(distance);
            pointColor = new Color(1 - value, 0, value);
            _colorManager.SetColor(_renderer, pointColor);
        }
    }

}

