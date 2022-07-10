using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeKnotsValue : MonoBehaviour
{
    GameManager gameManager;
    InputField indexField;
    InputField valueField;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        indexField = transform.Find("InsertTheNodeIndex").GetComponent<InputField>();
        valueField = transform.Find("InsertNumber").GetComponent<InputField>();
    }

    public void OnSendInput()
    {
        gameManager.ChangeKnotsValue(int.Parse(indexField.text), float.Parse(valueField.text));
    }
}
