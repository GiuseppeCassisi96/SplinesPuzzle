using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LevelType
{
    Bezier,
    Spline,
    Menu
}

public class SceneInfo : MonoBehaviour
{
    [Serializable]
    public struct Info
    {
        public SplineCurve CurveA, CurveB;
        public int pointsToWin;
        public AudioClip backgroundMusic, levelUnlock;
        public LevelType levelType;
        public ShowGameInfo gameInfo;
        public GameObject portal;
        public float tollerance;
        [Range(5, 100)]
        public int bezierResolution;
    }
    [SerializeField]
    Info info;

    public Info GetInfo()
    {
        return info;
    }

    

}
