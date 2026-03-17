using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //속도 조정 변수
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed; // walk랑 run을 대입하는 형식으로 하나의 변수로 이용하게 함

    //상태 변수
    private bool isRun = false;//뛰는지 아닌지 확인

    //민감도
    [SerializeField] private float LookSensitivity;

    //카메라 한계
    [SerializeField] private float CameraRotationLimit;
    private float currentCameraRotationX = 0;
    
    //필요 컴퓨넌트
    [SerializeField] private Camera theCamera;
    private Rigidbody myRigid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // LeftShift를 누르고 있으면
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) // LeftShift를 떼면
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirY = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirY;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * LookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRationX = _xRotation * LookSensitivity;
        currentCameraRotationX -= _cameraRationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -CameraRotationLimit, CameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
