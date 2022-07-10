using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class IncreaseParticles : MonoBehaviour
{

    PointInfo _pointInfo;
    Transform _tr;
    VFXManager _vfxManager;
    float _numberOfParticles;

    [SerializeField]
    string sizeParamName;
    [SerializeField]
    VisualEffect pointAuraVFX;
    
    

    private void Awake()
    {
        _pointInfo = GetComponent<PointInfo>();
        _vfxManager = GameObject.Find("VFXManager").GetComponent<VFXManager>();
        _tr = GetComponent<Transform>();
        _numberOfParticles = _vfxManager.GetFloatValueVar(sizeParamName, pointAuraVFX);
    }

    private void Update()
    {
        if(_pointInfo.isMoving)
        {
            _vfxManager.PlayVFX(pointAuraVFX);
            float distance = Vector2.Distance(_tr.localPosition,_pointInfo.desiredPosition);
            float value = _numberOfParticles / distance;
            _vfxManager.SetFloatValueVar(sizeParamName, value, pointAuraVFX);
        }
        else
        {
            _vfxManager.StopVFX(pointAuraVFX);
        }
    }
}
