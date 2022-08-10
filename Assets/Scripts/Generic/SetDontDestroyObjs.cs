using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDontDestroyObjs : MonoBehaviour
{
    [SerializeField]
    List<GameObject> objects;
    private void Start()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            DontDestroyOnLoad(objects[i]);
        }
    }
}
