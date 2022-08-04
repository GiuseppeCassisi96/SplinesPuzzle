using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuActivation : MonoBehaviour
{
    GameObject inGameUI, pauseUI, UI;
    GameManager gameManager;
    bool isPause = false;

    private void Awake()
    {
        UI = GameObject.Find("GameUI");
        inGameUI = UI.transform.Find("InGameUI").gameObject;
        pauseUI = UI.transform.Find("PauseUI").gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

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
                gameManager.mouseIsLock = !gameManager.mouseIsLock;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                gameManager.mouseIsLock = !gameManager.mouseIsLock;
                
            }
        }
    }
}
