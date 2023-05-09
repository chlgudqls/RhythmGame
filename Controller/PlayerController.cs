using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어가 Goal에 도달했으니 움직일수없게 함
    public static bool s_canPresskey = true;

    // 스피드, 뱡향, 목적지 필요
    [SerializeField] float moveSpeed = 3;
    Vector3 dir = new Vector3();
    public Vector3 destPos = new Vector3();
    Vector3 originPos = new Vector3();

    // 회전
    [SerializeField] float spinSpeed = 270;
    Vector3 rotDir = new Vector3();
    Quaternion destRot = new Quaternion();

    // 반동
    [SerializeField] float recoilPosY = 0.25f;
    [SerializeField] float recoilSpeed = 1.5f;

    bool canMove = true;
    bool isFalling = false;

    // 가짜 큐브를 돌려놓고, 그 돌아간 만큼의 값을 목표 회전값으로 삼는다고함
    [SerializeField] Transform fakeCube = null;
    [SerializeField] Transform realCube = null;

    TimingManager theTimingManager;
    CameraController theCam;
    Rigidbody myRigid;
    StatusManager theStatus;


    private void Start()
    {
        theTimingManager = FindObjectOfType<TimingManager>();
        theCam = FindObjectOfType<CameraController>();
        theStatus = FindObjectOfType<StatusManager>();
        myRigid = GetComponentInChildren<Rigidbody>();
        originPos = transform.position;
    }

    public void Initialized()
    {
        transform.position = Vector3.zero;
        destPos = Vector3.zero;
        realCube.localPosition = Vector3.zero;
        canMove = true;
        s_canPresskey = true;
        isFalling = false;
        myRigid.useGravity = false;
        myRigid.isKinematic = true;
    }

    void Update()
    {
        if (GameManager.instance.isStartGame)
        {
            CheckFalling();

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
            {
                if (canMove && s_canPresskey && !isFalling)
                {
                    Calc();
                    // 판정체크
                    // ※ 함수가 if안에 있어도 실행하는건 변함없다
                    if (theTimingManager.CheckTiming())
                    {
                        StartAction();
                    }
                }
            }
        }
    }
    private void Calc()
    {
        //방향 계산
        dir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        // 이동 목표값 계산
        destPos = transform.position + new Vector3(-dir.x, 0, dir.z);

        // 회전 목표값 계산
        // ※ 축에 따라 굴러가는 방향을 생각해서 -,+
        rotDir = new Vector3(-dir.z, 0f, -dir.x);
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
        destRot = fakeCube.rotation;
    }
    private void StartAction()
    {
        StartCoroutine(MoveCo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(theCam.ZoomCam());
    }

    IEnumerator MoveCo()
    {
        canMove = false;

        // ※ 제곱근을 이용, 
        while(Vector3.SqrMagnitude(transform.position - destPos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destPos;

        canMove = true;
    }

    IEnumerator SpinCo()
    {
        while(Quaternion.Angle(realCube.rotation, destRot) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, destRot, spinSpeed * Time.deltaTime);
            yield return null;
        }

        realCube.rotation = destRot;
    }

    // y 위치값을 증가시킨다 정해둔 일정값까지 그리고 0으로 원래값으로 되돌림 어떤 스피드만큼
    IEnumerator RecoilCo()
    {
        while(realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }
        while (realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }
        realCube.localPosition = new Vector3(0, 0, 0);
    }
    private void CheckFalling()
    {
        if (!isFalling && canMove)
        {
            if(!Physics.Raycast(transform.position, Vector3.down, 1.1f))
                Falling();
        }
    }
    private void Falling()
    {
        isFalling = true;
        myRigid.useGravity = true;
        myRigid.isKinematic = false;
    }

    public void ResetFalling()
    {
        theStatus.DecreaseHP(1);
        AudioManager.instance.PlaySFX("Falling");

        if (!theStatus.IsDead())
        {
            isFalling = false;
            myRigid.useGravity = false;
            myRigid.isKinematic = true;

            transform.position = originPos;
            realCube.localPosition = new Vector3(0, 0, 0);
        }
    }
}
