using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeLogic : MonoBehaviour
{

    Text timeText;
    int seconds = 1;
    WaitForSeconds wait;

    public Text TimeText
    {
        get { return timeText;}
        set { timeText = value;}
    }

    public int Seconds
    {
        set { seconds = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        timeText = GetComponent<Text>();
        wait = new WaitForSeconds(1);
    }

    public void SetActiveObj(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public IEnumerator TimeAdvance()
    {
        while (true)
        {
            yield return wait;
            timeText.text = seconds.ToString();
            seconds++;
        }
    }
}
