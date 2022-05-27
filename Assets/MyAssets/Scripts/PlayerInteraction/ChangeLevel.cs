using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField]
    int nextScene;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(nextScene);
    }
}
