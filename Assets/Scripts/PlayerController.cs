using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //속도 조정 변수
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed; // walk랑 run을 대입하는 형식으로 하나의 변수로 이용하게 함

    [SerializeField] private float crouchSpeed; // 앉기

    [SerializeField] private float jumpForce;

    //상태 변수
    private bool isRun = false;//뛰는지 아닌지 확인
    private bool isCrouch = false;
    private bool isGround = true;

    // 앉을때 얼마나 앉을지 결정하는 변수
    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 땅 착지 여부
    private CapsuleCollider capsuleCollider;

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
        capsuleCollider = GetComponent<CapsuleCollider>();

        //카메라 위치 초기화
        originPosY = theCamera.transform.localPosition.y; //카메라가 player 안에 포함되어잇기에 local사용
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    //점프 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    //부드러운 앉기 동작
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;

        int count = 0;
        while(_posY != applyCrouchPosY)
        {   
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            yield return null; // 1프레임 대기
            if(count>15) // 적당히 하고 끝내기
                break;
        }        
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0); // 원하는 값에 맞추기
    }

    //지면 체크
    private void IsGround()
    {
        //Physics.Raycast: 광선을 쏜다(현재 위치에서, 어디로(3차원공간에서 고정된down으로), 얼마만큼(캡슐의 공간값(y)의 절반)+여유)
        //닿으면 true
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+0.1f);
    }

    //점프 시도
    private void TryJump()
    {
        if(Input.GetKey(KeyCode.Space)&& isGround) //GetKeyDown은 누를때 한번만 GetKey는 누르고있으면 계속
        {
            Jump();
        }
    }

    //점프 동작
    private void Jump()
    {
        if(isCrouch) // 앉아있을경우에 점프하면 자동으로 앉기 해제
            Crouch();
        myRigid.linearVelocity = transform.up * jumpForce;
        
    }

    //달리기 시도
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

    //달리기 동작
    private void Running()
    {
        if(isCrouch) // 앉아있을경우에 달리기하면 자동 해제
            Crouch();
        isRun = true;
        applySpeed = runSpeed;
    }

    //달리기 치소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    //움직임 동작
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirY = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirY;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    //좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * LookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

    }

    //상하 카메라 회전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRationX = _xRotation * LookSensitivity;
        currentCameraRotationX -= _cameraRationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -CameraRotationLimit, CameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
