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
    PointInfo levelEvaluation;
    #endregion

    #region Serialize Field
    [SerializeField]
    GameObject portal;
    [SerializeField]
    SplinesCreation curveA, curveB;
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
    public bool mouseIsLock = false;
    [HideInInspector]
    public bool isInteractionWithCurve = false;
    [HideInInspector]
    public bool KnotsValueIsEquals = false;
    public LevelType levelType;
    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Screen.fullScreen = true;
        Cursor.visible = false;
        if (levelType == LevelType.Spline)
        {
            gameInfo.ShowKnotsValue();
        }
        levelEvaluation = GetComponent<PointInfo>();
    }

    void MouseLocks()
    {
        mouseIsLock = !mouseIsLock;
        if(mouseIsLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Screen.fullScreen = true;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Screen.fullScreen = false;
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        MouseRotation();
        if(Input.GetMouseButtonDown(1))
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


        if((index > 0) && (index < curveA.knots.nodes.Count - 1))
        {
            if ((value >= curveA.knots.nodes[index - 1])
            && (value <= curveA.knots.nodes[index + 1]))
            {
                curveA.knots.Substituite(value, index);
                isInteractionWithCurve = true;
                gameInfo.ShowKnotsValue();
            }
            else
            {
                gameInfo.ValueNotValid();
            }
        }
        else if(index == 0)
        {
            if (value <= curveA.knots.nodes[index + 1])
            {
                curveA.knots.Substituite(value, index);
                isInteractionWithCurve = true;
                gameInfo.ShowKnotsValue();
            }
            else
            {
                gameInfo.ValueNotValid();
            }
        }
        else if (index == curveA.knots.nodes.Count - 1)
        {
            if (value >= curveA.knots.nodes[index - 1])
            {
                curveA.knots.Substituite(value, index);
                isInteractionWithCurve = true;
                gameInfo.ShowKnotsValue();
            }
            else
            {
                gameInfo.ValueNotValid();
            }
        }
        ControlKnotsValue();
        
    }

    public bool ControlPointEval(Transform tr, Vector3 desiredPosition, float offsetEval)
    {
        bool xSide = (tr.localPosition.x >= (desiredPosition.x - offsetEval)) &&
           (tr.localPosition.x <= (desiredPosition.x + offsetEval));
        bool ySide = (tr.localPosition.y >= (desiredPosition.y - offsetEval)) &&
            (tr.localPosition.y <= (desiredPosition.y + offsetEval));
        return xSide && ySide;
    }

    bool ControlKnotsValue()
    {
        int lenght = curveA.knots.nodes.Count;
        if ((curveA.knots.multiplicityDict[curveA.knots.nodes[0]] == 
            curveB.knots.multiplicityDict[curveB.knots.nodes[0]]) && 
            (curveA.knots.multiplicityDict[curveA.knots.nodes[lenght-1]] ==
            curveB.knots.multiplicityDict[curveB.knots.nodes[lenght-1]]))
        {
            AddingPoint();
            return true;
        }
        return false;
    }


}
