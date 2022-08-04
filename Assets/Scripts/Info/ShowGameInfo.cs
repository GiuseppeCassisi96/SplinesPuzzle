using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowGameInfo : MonoBehaviour
{
    SplinesCreation curveA, curveB;
    [SerializeField]
    Text infoText;
    GameManager gameManager;

    private void Start()
    {
        infoText.text = infoText.text + "Knots value:\n";
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        curveA = gameManager.CurveA;
        curveB = gameManager.CurveB;
        for (int i = 0; i < curveA.knots.nodes.Count; i++)
        {
            infoText.text = infoText.text + curveA.knots.nodes[i] + " ";
        }
        infoText.text = infoText.text + "\n\n";
        infoText.text = infoText.text + "multiplicity curveA: " + curveA.multiplicity + "\n";
        infoText.text = infoText.text + "multiplicity curveB: " + curveB.multiplicity;
    }

    public void ShowKnotsValue()
    {
        infoText.text = infoText.text + "Knots value:\n";
        for(int i = 0; i < curveA.knots.nodes.Count; i++)
        {
            infoText.text = infoText.text + curveA.knots.nodes[i] + " ";
        }
        infoText.text = infoText.text + "\n\n";
    }

    public void ValueNotValid()
    {
        infoText.text = infoText.text + "Value not valid !\n";
    }
}
