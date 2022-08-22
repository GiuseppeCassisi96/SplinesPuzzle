using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTime : MonoBehaviour
{
    Text referenceText, myText;
    private void Start()
    {
        myText = GetComponent<Text>();
        referenceText = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        myText.text = myText.text + referenceText.text;
    }

}
