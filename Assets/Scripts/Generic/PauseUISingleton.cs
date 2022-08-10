using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUISingleton : MonoBehaviour
{
    static PauseUISingleton instance = null;
    [SerializeField]
    GameObject gameUI;
    [SerializeField]
    GameTimeLogic gameTimeLogic;
    WaitForSeconds timeWait;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            gameUI.SetActive(false);
            gameTimeLogic.SetActiveObj(true);
            gameTimeLogic.TimeText.text = "0";
            gameTimeLogic.Seconds = 0;
            gameTimeLogic.StopCoroutine("TimeAdvance");
        }
        else if(scene.buildIndex == 1)
        {
            gameUI.SetActive(true);
            gameTimeLogic.SetActiveObj(true);
            gameTimeLogic.TimeText.text = "0";
            gameTimeLogic.Seconds = 0;
            gameTimeLogic.StartCoroutine("TimeAdvance");
        }
    }

    private void Start()
    {
        Singleton();
        timeWait = new WaitForSeconds(1);
    }
    void Singleton()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject);
    }
}
