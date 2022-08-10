using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowGameInfo : MonoBehaviour
{
    SplineCurve curveA;
    [SerializeField]
    Text infoText;
    GameManager gameManager;

    private void Start()
    {
        infoText.text = infoText.text + "Knots value:\n";
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        curveA = gameManager.CurveA;
        for (int i = 0; i < curveA.NodesVector.Count; i++)
        {
            infoText.text = infoText.text + curveA.NodesVector[i] + " ";
        }
    }

    public void ShowKnotsValue()
    {
        infoText.text = infoText.text + "Knots value:\n";
        for (int i = 0; i < curveA.NodesVector.Count; i++)
        {
            infoText.text = infoText.text + curveA.NodesVector[i] + " ";
        }
        infoText.text = infoText.text + "\n\n";
    }

    public void ValueNotValid(System.Exception e)
    {
        infoText.text = infoText.text + e.Message + "\n";
    }
}
