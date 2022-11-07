using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class NK_PlayerMove : MonoBehaviourPun//, IPunObservable
{
    public enum State
    {
        Idle,
        Move,
        Sit,
    }

    public State state;

    // 애니메이션
    Animator anim;

    // 속력
    public float moveSpeed = 3;
    // 점프 파워
    public float jumpPower = 3;

    // CC
    CharacterController controller;
    // y방향 속력
    float yVelocity;
    // 중력
    float gravity = -9.81f;

    // 도착위치
    Vector3 receivePos;
    // 회전되야 하는 값
    Quaternion receiveRot;
    // 보간속력
    public float lerpSpeed = 100;

    // 얼굴카메라
    public GameObject faceCam;
    // 머리위에 왕관
    public GameObject crown;

    // 방에있는 인원확인 (RPC를 다시 또 쏴주기위해서)
    int playerIndex;

    public GameObject speaker;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

        // 선생님방에 있을 때
        if (GameObject.Find("GameManager"))
        {
            // 임시로 아이와 선생님 분류
            speaker.GetComponent<AudioSource>().mute = false;
            // 방 만든 사람(선생님)이 아닐 경우
            if (UserInfo.memberRole != "TEACHER")
            {
                gameObject.tag = "Child";
                GameManager.Instance.AddPlayer(photonView);
                if (photonView.IsMine)
                {
                    GameObject.Find("TeacherUI").SetActive(false);
                    GameObject.Find("BookBtn").SetActive(false);
                    GameManager.Instance.photonView = photonView;
                }
            }
            // 방 만든 사람(선생님)일 경우
            else
            {
                gameObject.tag = "Teacher";
                if (photonView.IsMine)
                {
                    GameObject.Find("ChildUI").SetActive(false);
                    GameManager.Instance.photonView = photonView;
                }
            }
        }
        else
        {
            speaker.GetComponent<AudioSource>().mute = true;
        }
        ///
        if (photonView.IsMine)
        {
            faceCam.SetActive(true);
            UserInfo.photonId = this.gameObject.GetComponent<PhotonView>().ViewID.ToString();

            //// 선생님이면 머리위에 왕관쓰기
            //if (UserInfo.memberRole == "TEACHER")
            //{
            //    photonView.RPC("RPCSetCrown", RpcTarget.All);
            //    //crown.SetActive(true);
            //}
        }

        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        state = State.Idle;
    }

    // 애니메이션 조절할 bool값
    bool moveBool = false;
    Vector3 movePoint;

    void Update()
    {
        //if (playerIndex != PhotonNetwork.CurrentRoom.Players.Count)
        //{
        //    // 선생님이면 머리위에 왕관쓰기
        //    if (UserInfo.memberRole == "TEACHER" && photonView.IsMine)
        //    {
        //        photonView.RPC("RPCSetCrown", RpcTarget.All);
        //        //crown.SetActive(true);
        //    }

        //    playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
        //}

        if (photonView.IsMine)
        {
            // 마우스로 이동하기
            if (Input.GetMouseButtonDown(0))
            {
                // 마우스 클릭 후 떼었을때 마우스 포지션으로 레이 생성
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawLine(ray.origin, ray.direction * 10f, Color.green, 1f);

                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    // UI를 선택한 경우와 플레이어를 선택한 경우가 아니라면
                    if (EventSystem.current.IsPointerOverGameObject() == false && raycastHit.transform.gameObject.layer != 6)
                    {
                        if (raycastHit.transform.gameObject.tag == "Room")
                        {
                            GameObject.Find("Canvas").transform.GetChild(12).gameObject.SetActive(true);
                            // 방정보 가져오기
                            YJ_DataManager.instance.goingRoomName = raycastHit.transform.gameObject.GetComponent<YJ_RoomTrigger>().roomName;
                            YJ_DataManager.instance.goingRoomType = raycastHit.transform.gameObject.GetComponent<YJ_RoomTrigger>().roomType;
                        }
                        else
                        {
                            movePoint = raycastHit.point;
                        }
                    }
                }
            }

            switch (state)
            {
                case State.Move:
                    //anim.SetBool("Move", moveBool);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Move", moveBool);
                    if (GameObject.Find("CreateRoomSet") != null || GameObject.Find("RoomList") != null)
                    {
                        return;
                    }
                    PlayerMouseMove();
                    //PlayerMove();
                    break;
                case State.Sit:
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Sit", true);
                    break;
                case State.Idle:
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Sit", false);

                    // 처음 입장 시 떨어지게 만들기
                    yVelocity += gravity * Time.deltaTime;
                    if (controller.isGrounded)
                    {
                        yVelocity = 0;
                    }
                    dir.Normalize();
                    dir.y = yVelocity;
                    controller.Move(dir * moveSpeed * Time.deltaTime);

                    // 어딘가 클릭했을때 무브로 이동
                    if (movePoint != Vector3.zero)
                    {
                        state = State.Move;
                    }
                    break;

            }
        }
    }

    Vector3 dir;
    float h = 0;
    float v = 0;
    int jumpCount = 0;

    void PlayerMouseMove()
    {

            dir = movePoint - transform.position;
            dir.Normalize();
        
        //dir.y = 0;

        // yVelocity값을 중력으로 감소시킴
         yVelocity += gravity * Time.deltaTime;
        // 만약에 바닥에 닿아있다면 yVelocity를 0으로 하자
        if (controller.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
        }

        // 스페이스바를 누르면 yVelocity에 jumpPower를 셋팅
        //if (Input.GetButtonDown("Jump") && jumpCount < 1)
        //{
        //    yVelocity = jumpPower;
        //    jumpCount++;
        //}
        dir.y = yVelocity;


        if (Vector3.Distance(movePoint, transform.position) < 0.1f )// || movePoint == Vector3.zero)
        {
            moveBool = false;
            return;
        }
        else if(Vector3.Distance(movePoint, transform.position) > 0.1f && movePoint == Vector3.zero )
        {
            moveBool = false;
        }
        else
        {
            moveBool = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * moveSpeed * 2);
            controller.Move(dir * moveSpeed * Time.deltaTime);
        }
    }

    void PlayerMove()
    {
        // 1. WSAD의 신호를 받자
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // 2. 받은 신호로 방향을 만든다
        dir = Camera.main.transform.forward * v + Camera.main.transform.right * h;
        dir.y = 0;
        dir.Normalize();
        if (dir.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        // yVelocity값을 중력으로 감소시킴
        yVelocity += gravity * Time.deltaTime;
        // 만약에 바닥에 닿아있다면 yVelocity를 0으로 하자
        if (controller.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
        }

        // 스페이스바를 누르면 yVelocity에 jumpPower를 셋팅
        if (Input.GetButtonDown("Jump") && jumpCount < 1)
        {
            yVelocity = jumpPower;
            jumpCount++;
        }


        dir.y = yVelocity;


        controller.Move(dir * moveSpeed * Time.deltaTime);
    }


    [PunRPC]
    public void RpcSetBool(string s, bool b)
    {
        if (anim != null)
            anim.SetBool(s, b);
    }

    [PunRPC]
    private void RPCLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
       // NK_TeacherManager.instance.JoinRoom();
    }

    [PunRPC]
    public void RPCSetCrown()
    {
        crown.SetActive(true);
    }
}