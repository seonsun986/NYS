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

    // �ִϸ��̼�
    Animator anim;

    // �ӷ�
    public float moveSpeed = 3;
    // ���� �Ŀ�
    public float jumpPower = 3;

    // CC
    CharacterController controller;
    // y���� �ӷ�
    float yVelocity;
    // �߷�
    float gravity = -9.81f;

    // ������ġ
    Vector3 receivePos;
    // ȸ���Ǿ� �ϴ� ��
    Quaternion receiveRot;
    // �����ӷ�
    public float lerpSpeed = 100;

    // ��ī�޶�
    public GameObject faceCam;
    // �Ӹ����� �հ�
    public GameObject crown;

    // �濡�ִ� �ο�Ȯ�� (RPC�� �ٽ� �� ���ֱ����ؼ�)
    int playerIndex;

    public GameObject speaker;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

        // �����Թ濡 ���� ��
        if (GameObject.Find("GameManager"))
        {
            // �ӽ÷� ���̿� ������ �з�
            speaker.GetComponent<AudioSource>().mute = false;
            // �� ���� ���(������)�� �ƴ� ���
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
            // �� ���� ���(������)�� ���
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

            //// �������̸� �Ӹ����� �հ�����
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

    // �ִϸ��̼� ������ bool��
    bool moveBool = false;
    Vector3 movePoint;

    void Update()
    {
        //if (playerIndex != PhotonNetwork.CurrentRoom.Players.Count)
        //{
        //    // �������̸� �Ӹ����� �հ�����
        //    if (UserInfo.memberRole == "TEACHER" && photonView.IsMine)
        //    {
        //        photonView.RPC("RPCSetCrown", RpcTarget.All);
        //        //crown.SetActive(true);
        //    }

        //    playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
        //}

        if (photonView.IsMine)
        {
            // ���콺�� �̵��ϱ�
            if (Input.GetMouseButtonDown(0))
            {
                // ���콺 Ŭ�� �� �������� ���콺 ���������� ���� ����
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawLine(ray.origin, ray.direction * 10f, Color.green, 1f);

                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    // UI�� ������ ���� �÷��̾ ������ ��찡 �ƴ϶��
                    if (EventSystem.current.IsPointerOverGameObject() == false && raycastHit.transform.gameObject.layer != 6)
                    {
                        if (raycastHit.transform.gameObject.tag == "Room")
                        {
                            GameObject.Find("Canvas").transform.GetChild(12).gameObject.SetActive(true);
                            // ������ ��������
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

                    // ó�� ���� �� �������� �����
                    yVelocity += gravity * Time.deltaTime;
                    if (controller.isGrounded)
                    {
                        yVelocity = 0;
                    }
                    dir.Normalize();
                    dir.y = yVelocity;
                    controller.Move(dir * moveSpeed * Time.deltaTime);

                    // ��� Ŭ�������� ����� �̵�
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

        // yVelocity���� �߷����� ���ҽ�Ŵ
         yVelocity += gravity * Time.deltaTime;
        // ���࿡ �ٴڿ� ����ִٸ� yVelocity�� 0���� ����
        if (controller.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
        }

        // �����̽��ٸ� ������ yVelocity�� jumpPower�� ����
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
        // 1. WSAD�� ��ȣ�� ����
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // 2. ���� ��ȣ�� ������ �����
        dir = Camera.main.transform.forward * v + Camera.main.transform.right * h;
        dir.y = 0;
        dir.Normalize();
        if (dir.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        // yVelocity���� �߷����� ���ҽ�Ŵ
        yVelocity += gravity * Time.deltaTime;
        // ���࿡ �ٴڿ� ����ִٸ� yVelocity�� 0���� ����
        if (controller.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
        }

        // �����̽��ٸ� ������ yVelocity�� jumpPower�� ����
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