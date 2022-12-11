using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.SceneManagement;

public class NK_PlayerMove : MonoBehaviourPun//, IPunObservable
{
    public enum State
    {
        Idle,
        Move,
        Sit,
        HandUp,
        List,
    }

    public State state;

    // �ִϸ��̼�
    public Animator anim;

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
        if (this.gameObject.layer != 5)
        {
            if (SceneManager.GetActiveScene().name != "MyRoomScene")
                playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

            // �����Թ濡 ���� ��
            if (GameObject.Find("GameManager"))
            {
                // �ӽ÷� ���̿� ������ �з�
                speaker.GetComponent<AudioSource>().mute = false;
                // �� ���� ���(������)�� �ƴ� ���
                if (YJ_DataManager.instance.myInfo.memberRole != "TEACHER")
                {
                    if (photonView.IsMine)
                    {
                        photonView.RPC("RPCAddPlayer", RpcTarget.AllBuffered);
                        photonView.RPC("RPCSetTag", RpcTarget.All, "Child");
                        GameObject.Find("TeacherUI").SetActive(false);
                        GameObject.Find("BookBtn").SetActive(false);
                        GameManager.Instance.photonView = photonView;
                    }
                }
                // �� ���� ���(������)�� ���
                else
                {
                    if (photonView.IsMine)
                    {
                        photonView.RPC("RPCSetTag", RpcTarget.All, "Teacher");
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
                //UserInfo_e.photonId = this.gameObject.GetComponent<PhotonView>().ViewID.ToString();     
            }

        }

        controller = GetComponent<CharacterController>();


    }

    public void SetAnim(int animalId)
    {
        anim = transform.GetChild(animalId).GetComponent<Animator>();
        state = State.Idle;
    }

    public void SetListAnim(int animalId)
    {
        print("������");
        anim = transform.GetChild(animalId).GetComponent<Animator>();
        state = State.List;
    }

    // �ִϸ��̼� ������ bool��
    bool moveBool = false;
    Vector3 movePoint;
    public GameObject footPrintFactory;
    public GameObject playerCanvas;

    void Update()
    {
        if (playerCanvas != null)
        {
            if (state == State.Sit)
            {
                playerCanvas.SetActive(false);
            }
            else
                playerCanvas.SetActive(true);

        }

        if (photonView.IsMine && this.gameObject.layer != 5)
        {
            if (SceneManager.GetActiveScene().name != "MyRoomScene")
            {
                // ���콺�� �̵��ϱ�
                if (Input.GetMouseButtonDown(0))
                {
                    // ���콺 Ŭ�� �� �������� ���콺 ���������� ���� ����
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Debug.DrawLine(ray.origin, ray.direction * 10f, Color.green, 1f);

                    if (Physics.Raycast(ray, out RaycastHit raycastHit))
                    {
                        Debug.Log(raycastHit.collider.gameObject.name);
                        // ������ ��������
                        if (raycastHit.transform.gameObject.tag == "Animal")
                        {
                            raycastHit.transform.gameObject.GetComponent<YJ_PlazaAnimal>().state = YJ_PlazaAnimal.State.Interaction;
                            raycastHit.transform.gameObject.GetComponent<YJ_PlazaAnimal>().player = transform.position;
                        }
                        // ���� ���ϴ� ���� ������ ��
                        else if(raycastHit.transform.gameObject.tag == "Dont")
                        {
                            return;
                        }
#if UNITY_ANDROID

                        else if (Input.touchCount < 1)
                        {
                            if (raycastHit.transform.gameObject.layer != 6)
                            {
                            movePoint = raycastHit.point;
                            GameObject footPrint = Instantiate(footPrintFactory);
                            footPrint.transform.position = raycastHit.point;
                            Vector3 dir = this.gameObject.transform.position - footPrint.transform.position;
                            dir.y = 0;
                            footPrint.transform.forward = -dir;
                            }
                        }
                        else if (Input.touchCount > 0 && raycastHit.transform.gameObject.layer != 6 )
                        {
                            if(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                            {
                                if (raycastHit.transform.gameObject.tag == "Room")
                                {
                                    GameObject.Find("Canvas").transform.Find("PopUpBG").gameObject.SetActive(true);
                                    GameObject.Find("Canvas").transform.GetChild(15).gameObject.SetActive(true);
                                    // ������ ��������
                                    YJ_DataManager.instance.goingRoomName = raycastHit.transform.gameObject.GetComponent<YJ_RoomTrigger>().roomName;
                                    YJ_DataManager.instance.goingRoomType = raycastHit.transform.gameObject.GetComponent<YJ_RoomTrigger>().roomType;
                                }
                                else
                                {
                                    movePoint = raycastHit.point;
                                    GameObject footPrint = Instantiate(footPrintFactory);
                                    footPrint.transform.position = raycastHit.point;
                                    Vector3 dir = this.gameObject.transform.position - footPrint.transform.position;
                                    dir.y = 0;
                                    footPrint.transform.forward = -dir;
                                }
                            }
                        }
#else
                        // UI�� ������ ���� �÷��̾ ������ ��찡 �ƴ϶��
                        else if (EventSystem.current.IsPointerOverGameObject() == false && raycastHit.transform.gameObject.layer != 6)

                        {
                            if (raycastHit.transform.gameObject.tag == "Room")
                            {
                                GameObject.Find("Canvas").transform.Find("PopUpBG").gameObject.SetActive(true);
                                GameObject.Find("Canvas").transform.GetChild(15).gameObject.SetActive(true);
                                // ������ ��������
                                YJ_DataManager.instance.goingRoomName = raycastHit.transform.gameObject.GetComponent<YJ_RoomTrigger>().roomName;
                                YJ_DataManager.instance.goingRoomType = raycastHit.transform.gameObject.GetComponent<YJ_RoomTrigger>().roomType;
                            }
                            else
                            {
                                movePoint = raycastHit.point;
                                GameObject footPrint = Instantiate(footPrintFactory);
                                footPrint.transform.position = raycastHit.point;
                                Vector3 dir = this.gameObject.transform.position - footPrint.transform.position;
                                dir.y = 0;
                                footPrint.transform.forward = -dir;
                            }
                        }
#endif
                    }
                }
            }

            switch (state)
            {
                case State.Move:
                    //anim.SetBool("Move", moveBool);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Move", moveBool);
                    if (GameObject.Find("CreateRoomSet") != null)// || GameObject.Find("RoomList") != null)
                    {
                        return;
                    }
                    PlayerMouseMove();
                    //PlayerMove();
                    break;
                case State.Sit:
                    photonView.RPC("RpcSetBool", RpcTarget.All, "HandUp", false);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Sit", true);
                    sit = true;
                    break;
                case State.HandUp:
                    photonView.RPC("RpcSetBool", RpcTarget.All, "HandUp", true);
                    break;
                case State.Idle:
                    if (sit)
                    {
                        photonView.RPC("RpcSetBool", RpcTarget.All, "Sit", false);
                        photonView.RPC("RpcSetBool", RpcTarget.All, "HandUp", false);
                        movePoint = Vector3.zero;
                        dir = Vector3.zero;
                        sit = false;
                    }

                    // ó�� ���� �� �������� �����
                    yVelocity += gravity * Time.deltaTime;
                    if (controller != null && controller.isGrounded)
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
                case State.List:
                    photonView.RPC("RpcSetBool", RpcTarget.All, "ListHi", true);
                    break;

            }
        }
    }

    bool sit = false;

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


        if (Vector3.Distance(movePoint, transform.position) < 0.1f)// || movePoint == Vector3.zero)
        {
            moveBool = false;
            return;
        }
        else if (Vector3.Distance(movePoint, transform.position) > 0.1f && movePoint == Vector3.zero)
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

    [PunRPC]
    public void RPCSetTag(string tag)
    {
        gameObject.tag = tag;
    }

    [PunRPC]
    public void RPCAddPlayer()
    {
        GameManager.Instance.AddPlayer(photonView);
    }
}