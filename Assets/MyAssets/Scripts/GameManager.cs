using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelType
{
    Bezier,
    Spline
}

public class GameManager : MonoBehaviour
{
    public static int SPLINE_GRADE = 3;

    #region private var
    int n = 0;
    LevelType levelType;
    LevelEvaluation levelEvaluation;
    #endregion

    #region Serialize Field
    [SerializeField]
    GameObject portal;
    [SerializeField]
    SplinesCreation curve;
    [SerializeField]
    ShowGameInfo gameInfo;
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
    [HideInInspector]
    public bool KnotsValueIsEquals = false;
    #endregion

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(levelType == LevelType.Spline)
        {
            gameInfo.ShowKnotsValue();
        }
        levelEvaluation = GetComponent<LevelEvaluation>();
        
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
        Debug.Log("Punto in più");
        n++;
        if(n == numberOfPoints)
        {
            portal.SetActive(true);
        }
    }

    public void ChangeKnotsValue(int index, float value)
    {
        if(value < 0)
        {
            return;
        }


        if((index > 0) && (index < curve.knots.nodes.Count - 1))
        {
            if ((value >= curve.knots.nodes[index - 1])
            && (value <= curve.knots.nodes[index + 1]))
            {
                curve.knots.Substituite(value, index);
                pointIsMoving = true;
                gameInfo.ShowKnotsValue();
            }
            else
            {
                gameInfo.ValueNotValid();
            }
        }
        else if(index == 0)
        {
            if (value <= curve.knots.nodes[index + 1])
            {
                curve.knots.Substituite(value, index);
                pointIsMoving = true;
                gameInfo.ShowKnotsValue();
            }
            else
            {
                gameInfo.ValueNotValid();
            }
        }
        else if (index == curve.knots.nodes.Count - 1)
        {
            if (value >= curve.knots.nodes[index - 1])
            {
                curve.knots.Substituite(value, index);
                pointIsMoving = true;
                gameInfo.ShowKnotsValue();
            }
            else
            {
                gameInfo.ValueNotValid();
            }
        }
        levelEvaluation.ControlKnotsValue();
        
    }

    
}
