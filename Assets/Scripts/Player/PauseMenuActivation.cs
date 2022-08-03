using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuActivation : MonoBehaviour
{
    [SerializeField]
    GameObject inGameUI, pauseUI;
    bool isPause = false;
    [SerializeField]
    GameManager gameManager;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            
            isPause = !isPause;
            pauseUI.SetActive(isPause);
            inGameUI.SetActive(!isPause);
            
            if(isPause)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            }
        }
    }
}
