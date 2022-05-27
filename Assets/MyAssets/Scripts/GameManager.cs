using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    

    int n = 0;

    private void OnEnable()
    {
        MovePoint.addingPoint += AddingPoint;
    }

    private void OnDisable()
    {
        MovePoint.addingPoint -= AddingPoint;
    }

    [SerializeField]
    GameObject portal;

    [Range(5,100)]
    public int CURVE_RESOLUTION = 50;
    public float xAxeMouse, yAxeMouse;
    public float pointMovementSpeed = 8.0f;
    public float tollerance = 0.2f;
    public int numberOfPoints = 3;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MouseRotation();
    }

    void MouseRotation()
    {
        xAxeMouse = Input.GetAxis("Mouse X");
        yAxeMouse = Input.GetAxis("Mouse Y");
    }

    public void AddingPoint()
    {
        n++;
        Debug.Log(n);
        if(n == numberOfPoints)
        {
            portal.SetActive(true);
        }
    }
}
