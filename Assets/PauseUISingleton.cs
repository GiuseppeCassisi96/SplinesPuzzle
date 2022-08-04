using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUISingleton : MonoBehaviour
{
    static PauseUISingleton instance = null;
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
