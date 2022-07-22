using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{    
    public Color GetColor(Renderer renderer)
    {
        return renderer.material.color;
    }

    public void SetColor(Renderer renderer, Color colorValue)
    {
        renderer.material.color = colorValue;
    }
        
}
