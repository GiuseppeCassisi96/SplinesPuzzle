using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowGameInfo : MonoBehaviour
{
    [SerializeField]
    SplinesCreation curve;
    [SerializeField]
    Text infoText;

    private void Awake()
    {
        infoText.text = infoText.text + "Knots value:\n";
        for (int i = 0; i < curve.knots.nodes.Count; i++)
        {
            infoText.text = infoText.text + curve.knots.nodes[i] + " ";
        }
    }

    public void ShowKnotsValue()
    {
        infoText.text = infoText.text + "Knots value:\n";
        for(int i = 0; i < curve.knots.nodes.Count; i++)
        {
            infoText.text = infoText.text + curve.knots.nodes[i] + " ";
        }
        infoText.text = infoText.text + "\n";
    }

    public void ValueNotValid()
    {
        infoText.text = infoText.text + "Value not valid !\n";
    }
}
