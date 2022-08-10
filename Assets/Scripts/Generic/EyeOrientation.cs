using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeOrientation : MonoBehaviour
{
    Transform _tr;
    [SerializeField]
    Transform pgTransform;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
    }

    private void Update()
    {
        _tr.LookAt(pgTransform, Vector3.right + Vector3.up);
    }
}
