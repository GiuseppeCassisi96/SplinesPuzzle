using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region private var
    Transform _tr;
    Transform _cameraTransform;
    Rigidbody _playerBody;
    GameManager _gameManager;
    int _jumpCount = 2;
    Ray ray;
    float _rotationX, _rotationY;
    Vector3 checkerScale;
    #endregion


    #region SerializeField var
    [SerializeField]
    float speed = 5.0f, jumpForce = 5.0f;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    Transform checkereTr;
    [SerializeField]
    AudioClip jumpClip;
    #endregion


    static bool isLook = false;

    public static bool IsLook
    {
        get { return isLook; }
        set { isLook = value; }
    }

    private void OnEnable()
    {
        EventManager.LookEvent += LookPoint;
    }

    private void OnDisable()
    {
        EventManager.LookEvent -= LookPoint;
    }


    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _cameraTransform = transform.Find("Camera");
        _playerBody = GetComponent<Rigidbody>();
        ray = new Ray(_tr.position, -_tr.up * 1.5f);
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        checkerScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float xAxe = Input.GetAxis("Horizontal");
        float zAxe = Input.GetAxis("Vertical");
        _tr.Translate(Vector3.forward * zAxe * speed * Time.deltaTime);
        _tr.Translate(Vector3.right * xAxe * speed * Time.deltaTime);
        //Jump and ground check section
        if (Input.GetKeyDown(KeyCode.Space) && _jumpCount > 1)
        {
            _playerBody.AddForce(_tr.up * jumpForce, ForceMode.Impulse);
            _jumpCount--;
            EventManager.PlaySoundSFXAction(jumpClip);
        }
        ray.origin = _tr.position;
        ray.direction = -_tr.up * 1.5f;
        if(Physics.CheckBox(checkereTr.position, checkerScale, Quaternion.identity, mask))
        {
            _jumpCount = 2;
        }

       if(_gameManager.mouseIsLock)
        {
            //Rotation
            if (!isLook)
            {
                _rotationY += Vector3.up.y * _gameManager.xAxeMouse * _gameManager.mouseSensibility * Time.deltaTime;
                _rotationX += Vector3.right.x * (-_gameManager.yAxeMouse) * _gameManager.mouseSensibility * Time.deltaTime;
                _rotationX = Mathf.Clamp(_rotationX, -90, 80);
                _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
                _tr.localRotation = Quaternion.Euler(0, _rotationY, 0);
            }
            else
            {
                _rotationY = _cameraTransform.rotation.eulerAngles.y - 360;
                _rotationX = _cameraTransform.rotation.eulerAngles.x - 360;
                _rotationX = Mathf.Clamp(_rotationX, -90, 80);
            }
        }
        
    }

    void LookPoint(Transform tr)
    {
        _cameraTransform.LookAt(tr, Vector3.up);
    }

}
