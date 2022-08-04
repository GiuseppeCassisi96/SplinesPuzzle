using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUISingleton : MonoBehaviour
{
    static PauseUISingleton instance = null;
    [SerializeField]
    GameObject gameUI;

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
        }
        else
        {
            gameUI.SetActive(true);
        }
    }

    private void Start()
    {
        Singleton();
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
