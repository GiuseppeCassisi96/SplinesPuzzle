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
    public Color pointColor;
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
        if (_pointInfo.isMoving)
        {
            float distance = Vector2.Distance(_tr.localPosition, _pointInfo.desiredPosition);
            float value = Mathf.Clamp01(distance);
            pointColor = new Color(1 - value, 0, value);
            _colorManager.SetColor(_renderer, pointColor);
        }
    }

}













//PointInfo _pointInfo;
//Transform _tr;
//VFXManager _vfxManager;
//float _numberOfParticles;

//[SerializeField]
//string sizeParamName;
//[SerializeField]
//ParticleSystem pointAuraVFX;



//private void Awake()
//{
//    _pointInfo = GetComponent<PointInfo>();
//    _vfxManager = GameObject.Find("VFXManager").GetComponent<VFXManager>();
//    _tr = GetComponent<Transform>();
//    _numberOfParticles = _vfxManager.GetEmissionRate(pointAuraVFX).constant;
//}

//private void Update()
//{
//    if(_pointInfo.isMoving)
//    {
//        _vfxManager.PlayVFX(pointAuraVFX);
//        float distance = Vector2.Distance(_tr.localPosition,_pointInfo.desiredPosition);
//        float value = _numberOfParticles / distance;
//        _vfxManager.SetEmissionRate(pointAuraVFX, value);
//        Debug.Log("valore: " + value);
//    }
//    else
//    {
//        _vfxManager.StopVFX(pointAuraVFX);
//    }
//}
