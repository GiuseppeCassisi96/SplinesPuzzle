using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int SPLINE_GRADE = 3;

    #region private var
    int numberOfPoints = 3;
    SceneInfo sceneInfo;
    SplinesCreation curveA, curveB;
    GameObject portal;
    AudioClip backgroundClip, levelUnlock;
    ShowGameInfo gameInfo;
    float _tollerance = 0.2f;
    public int _bezierResolution = 50;
    #endregion

    #region public var
    [Range(5, 100)]
    
    [HideInInspector]
    public float xAxeMouse, yAxeMouse;
    [HideInInspector]
    public float pointMovementSpeed = 8.0f;
    [HideInInspector]
    public bool mouseIsLock = false;
    [HideInInspector]
    public bool isInteractionWithCurve = false;
    [HideInInspector]
    public bool KnotsValueIsEquals = false;
    public int mouseSensibility = 70;
    #endregion

    [SerializeField]
    Slider pointSpeed, mouse;

    #region Properties
    public float Tollerance
    {
        get
        {
            return _tollerance;
        }

        set
        {
            _tollerance = value;
        }
    }

    public int BezierResolution
    {
        get
        {
            return _bezierResolution;
        }

        set
        {
            _bezierResolution = value;
        }
    }

    public SplinesCreation CurveA
    {
        get
        {
            return curveA;
        }
    }

    public SplinesCreation CurveB
    {
        get
        {
            return curveB;
        }
    }
    #endregion



    private void OnEnable()
    {
        SceneManager.sceneLoaded += IsSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= IsSceneLoaded;
    }

    private void Awake()
    {
        pointMovementSpeed = pointSpeed.value;
        mouseSensibility = (int) mouse.value;
        DontDestroyOnLoad(this.gameObject);
    }

  

    private void Update()
    {
        MouseRotation();
        if(Input.GetKeyDown(KeyCode.L) && !(sceneInfo.GetInfo().levelType == LevelType.Menu))
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
        numberOfPoints++;
        if(numberOfPoints == sceneInfo.GetInfo().pointsToWin)
        {
            portal.SetActive(true);
            EventManager.PlaySoundSFXAction(levelUnlock);
        }
    }

    private void IsSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        sceneInfo = GameObject.FindGameObjectWithTag("SceneInfo").GetComponent<SceneInfo>();
        numberOfPoints = 0;
        if (sceneInfo.GetInfo().levelType == LevelType.Menu)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            backgroundClip = sceneInfo.GetInfo().backgroundMusic;
        }
        else if (sceneInfo.GetInfo().levelType == LevelType.Spline)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gameInfo = sceneInfo.GetInfo().gameInfo;

            curveA = sceneInfo.GetInfo().CurveA;
            curveB = sceneInfo.GetInfo().CurveB;

            portal = sceneInfo.GetInfo().portal;
            Tollerance = sceneInfo.GetInfo().tollerance;
            backgroundClip = sceneInfo.GetInfo().backgroundMusic;
            levelUnlock = sceneInfo.GetInfo().levelUnlock;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            portal = sceneInfo.GetInfo().portal;
            Tollerance = sceneInfo.GetInfo().tollerance;
            backgroundClip = sceneInfo.GetInfo().backgroundMusic;
            levelUnlock = sceneInfo.GetInfo().levelUnlock;

            BezierResolution = sceneInfo.GetInfo().bezierResolution;
        }
        EventManager.PlaySoundAction(backgroundClip);
    }


    void MouseLocks()
    {
        mouseIsLock = !mouseIsLock;
        if (mouseIsLock)
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
        bool mulEval = (curveA.knots.multiplicityDict[curveA.knots.nodes[0]] == curveB.knots.multiplicityDict[curveB.knots.nodes[0]]) &&
            (curveA.knots.multiplicityDict[curveA.knots.nodes[lenght - 1]] == curveB.knots.multiplicityDict[curveB.knots.nodes[lenght - 1]]);
        bool valueEval = (curveA.knots.nodes[0] == curveB.knots.nodes[0]) && (curveA.knots.nodes[lenght - 1] == curveB.knots.nodes[lenght - 1]);
        if (mulEval && valueEval)
        {  
            AddingPoint();
            return true;
        }
        return false;
    }

    public void ChangeMouseSensibility(Slider slider) 
    {
        mouseSensibility = (int)slider.value;
    }

    public void ChangePointSpeed(Slider slider)
    {
        pointMovementSpeed = (int)slider.value;
    }


}
