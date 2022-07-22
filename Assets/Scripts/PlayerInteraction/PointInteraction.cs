using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInteraction : MonoBehaviour
{
    Ray _ray;
    Transform _tr;
    RaycastHit _hit;
    int _triggerId;

    [SerializeField]
    int rayLenght;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    Camera cam;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _ray.direction = cam.transform.forward * rayLenght;
        _ray.origin = _tr.position;
    }

    private void Update()
    {
        _ray.direction = cam.transform.forward * rayLenght;
        _ray.origin = _tr.position;
        if(Physics.Raycast(_ray, out _hit, rayLenght, mask))
        {
            _triggerId = _hit.collider.gameObject.GetInstanceID();
            EventManager.EnterAction(_triggerId);
        }
        else
        {
            EventManager.ExitAction(_triggerId);
        }
        
    }
}
