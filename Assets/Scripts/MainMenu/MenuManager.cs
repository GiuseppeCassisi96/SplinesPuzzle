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
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        EventManager.PlaySoundSFXAction(UI_Clip);
    }
}
