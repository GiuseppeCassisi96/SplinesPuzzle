using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;

    #region private var
    int numberOfPoints = 0, pointsToUnlockPanels = 0;
    SceneInfo sceneInfo;
    SplineCurve curveA, curveB;
    GameObject portal;
    AudioClip backgroundClip, levelUnlock;
    ShowGameInfo gameInfo;
    float _tollerance = 0.2f;
    int _curveResolution = 50;
    WaitForSeconds _wait;
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

    public int CurveResolution
    {
        get
        {
            return _curveResolution;
        }

        set
        {
            _curveResolution = value;
        }
    }

    public SplineCurve CurveA
    {
        get
        {
            return curveA;
        }
    }

    public SplineCurve CurveB
    {
        get
        {
            return curveB;
        }
    }
    #endregion


    #region unity methods
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
    }

    private void Start()
    {
        Singleton();
        _wait = new WaitForSeconds(0.200f);
    }

    private void Update()
    {
        MouseRotation();
        if(Input.GetKeyDown(KeyCode.L) && !(sceneInfo.GetInfo().levelType == LevelType.Menu))
        {
            MouseLocks();
        }

    }
    #endregion

    #region user define methods
    void MouseRotation()
    {
        xAxeMouse = Input.GetAxis("Mouse X");
        yAxeMouse = Input.GetAxis("Mouse Y");
    }

    public void AddingPoint()
    {
        Debug.Log("Punto in più");
        numberOfPoints++;
        pointsToUnlockPanels++;
        if(numberOfPoints == sceneInfo.GetInfo().pointsToWin)
        {
            portal.SetActive(true);
            EventManager.PlaySoundSFXAction(levelUnlock);
            return;
        }
        if (sceneInfo.GetInfo().levelType == LevelType.Spline)
        {
            if(pointsToUnlockPanels == sceneInfo.GetInfo().pointsToUnlockPanels)
            {
                sceneInfo.GetInfo().infoPanel.SetActive(true);
                sceneInfo.GetInfo().interactionPanel.SetActive(true);
            }
            CurveA.ReplaceJunction();
            CurveA.DrawSpline();
        }
    }

    private void IsSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        sceneInfo = GameObject.FindGameObjectWithTag("SceneInfo").GetComponent<SceneInfo>();
        //reset the points for the new level
        numberOfPoints = 0;
        pointsToUnlockPanels = 0;
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

            CurveResolution = sceneInfo.GetInfo().bezierResolution;
        }
        EventManager.PlaySoundAction(backgroundClip);
    }


    private void MouseLocks()
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


    public void GMChangeKnotsValue(int index, float value)
    {
        try
        {
            curveA.ChangeKnotsValue(index, value);
            gameInfo.ShowKnotsValue();
            isInteractionWithCurve = true;
            StartCoroutine(ResetInteractionAfterTime(_wait));
        }
        catch(System.Exception e)
        {
            gameInfo.ValueNotValid(e);
        }
            
    }

    public bool GMControlPointEval(Transform tr, Vector3 desiredPosition, float offsetEval)
    {
        bool xSide = (tr.localPosition.x >= (desiredPosition.x - offsetEval)) &&
           (tr.localPosition.x <= (desiredPosition.x + offsetEval));
        bool ySide = (tr.localPosition.y >= (desiredPosition.y - offsetEval)) &&
            (tr.localPosition.y <= (desiredPosition.y + offsetEval));
        return xSide && ySide;
    }


    public void GMChangeMouseSensibility(Slider slider) 
    {
        mouseSensibility = (int)slider.value;
    }

    public void GMChangePointSpeed(Slider slider)
    {
        pointMovementSpeed = (int)slider.value;
    }

    private void Singleton()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject);
    }


    private IEnumerator ResetInteractionAfterTime(WaitForSeconds wait)
    {
        yield return wait;
        isInteractionWithCurve = false;
        yield return null;
    }
    #endregion
}
