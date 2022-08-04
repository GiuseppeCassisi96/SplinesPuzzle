using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject hideSection, viewSection;
    [SerializeField]
    AudioClip UI_Clip;
    [SerializeField]
    GameManager gameManager;

    public void HideAndView()
    {
        viewSection.SetActive(true);
        hideSection.SetActive(false);
        EventManager.PlaySoundSFXAction(UI_Clip);
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        EventManager.PlaySoundSFXAction(UI_Clip);
        Time.timeScale = 1;
    }

    public void InitTheGame()
    {
        SceneManager.LoadScene(1);
        EventManager.PlaySoundSFXAction(UI_Clip);
        Time.timeScale = 1;
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
        EventManager.PlaySoundSFXAction(UI_Clip);
        Time.timeScale = 1;
        gameManager.mouseIsLock = true;

    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        EventManager.PlaySoundSFXAction(UI_Clip);
        Time.timeScale = 1;
    }
}
