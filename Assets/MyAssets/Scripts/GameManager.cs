using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int SPLINE_GRADE = 3;

    #region private var
    int n = 0;
    #endregion

    #region Serialize Field
    [SerializeField]
    GameObject portal;
    [SerializeField]
    SplinesCreation curve;
    #endregion

    #region public var
    [Range(5, 100)]
    public int CURVE_RESOLUTION = 50;
    public float xAxeMouse, yAxeMouse;
    public float pointMovementSpeed = 8.0f;
    public float tollerance = 0.2f;
    public int numberOfPoints = 3;
    [HideInInspector]
    public bool mouseIsLock = true;
    [HideInInspector]
    public bool pointIsMoving = false;
    #endregion

    private void OnEnable()
    {
        MovePoint.addingPoint += AddingPoint;
    }

    private void OnDisable()
    {
        MovePoint.addingPoint -= AddingPoint;
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void MouseLocks()
    {
        mouseIsLock = !mouseIsLock;
        if(mouseIsLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        MouseRotation();
        if(Input.GetKeyDown(KeyCode.L))
        {
            MouseLocks();
        }
    }

    void MouseRotation()
    {
        xAxeMouse = Input.GetAxis("Mouse X");
        yAxeMouse = Input.GetAxis("Mouse Y");
    }

    public void AddingPoint()
    {
        n++;
        if(n == numberOfPoints)
        {
            portal.SetActive(true);
        }
    }

    public void ChangeKnotsValue(int index, float value)
    {
        if((value >= curve.knots[index - 1]) 
            && (value <= curve.knots[index + 1]))
        {
            curve.knots[index] = value;
            pointIsMoving = true;
        }
        else
        {
            Debug.LogWarning("Value not valid");
        }
    }

    
}
