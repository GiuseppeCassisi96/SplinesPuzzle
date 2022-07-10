using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    public void PlayVFX(VisualEffect VFX)
    {
        VFX.Play();
    }

    public void StopVFX(VisualEffect VFX)
    {
        VFX.Stop();
    }

    public void RestartVFX(VisualEffect VFX)
    {
        VFX.Reinit();
    }

    public void SetFloatValueVar(string varName, float value ,VisualEffect VFX)
    {
        VFX.SetFloat(varName, value);
    }

    public float GetFloatValueVar(string varName, VisualEffect VFX)
    {
        return VFX.GetFloat(varName);
    }

    public void SetIntValueVar(string varName, int value, VisualEffect VFX)
    {
        VFX.SetInt(varName, value);
    }

    public void SetVector3ValueVar(string varName, Vector3 value, VisualEffect VFX)
    {
        VFX.SetVector3(varName, value);
    }
}
