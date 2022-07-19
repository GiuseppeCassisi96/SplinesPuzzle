using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //private var
    Transform _tr;
    Transform _cameraTransform;
    Rigidbody _playerBody;
    GameManager _gameManager;
    int _jumpCount = 2;
    Ray ray;
    float _rotationX;
    Vector3 checkerScale;
    


    //SerializeField var
    [SerializeField]
    float speed = 5.0f, jumpForce = 5.0f, rotationSpeed = 2.0f;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    Transform checkereTr;

    public static bool _isLook = false;

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
        _cameraTransform = transform.Find("Main Camera");
        _playerBody = GetComponent<Rigidbody>();
        ray = new Ray(_tr.position, -_tr.up * 1.5f);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        }
        ray.origin = _tr.position;
        ray.direction = -_tr.up * 1.5f;
        if(Physics.CheckBox(checkereTr.position, checkerScale, Quaternion.identity, mask))
        {
            _jumpCount = 2;
        }

        //Rotation
        if(!_isLook)
        {
            _tr.Rotate(Vector3.up * _gameManager.xAxeMouse * rotationSpeed * Time.deltaTime);
            _rotationX += Vector3.right.x * -_gameManager.yAxeMouse * rotationSpeed * Time.deltaTime;
            _rotationX = Mathf.Clamp(_rotationX, -90, 80);
            _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        }
    }

    void LookPoint(Transform tr)
    {
        Debug.Log("PIPPO");
        _cameraTransform.LookAt(tr, Vector3.up);
        _isLook = true;
    }
}
