using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    public GameObject speaker;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // �����Թ濡 ���� ��
        if (GameObject.Find("GameManager"))
        {
            // �ӽ÷� ���̿� ������ �з�
            speaker.GetComponent<AudioSource>().mute = false;
            // �� ���� ���(������)�� �ƴ� ���
            if (PhotonNetwork.MasterClient.NickName != photonView.Owner.NickName)
            {
                gameObject.tag = "Child";
                GameManager.Instance.AddPlayer(photonView);
                if (photonView.IsMine)
                {
                    GameObject.Find("TeacherUI").SetActive(false);
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

        if (photonView.IsMine)
        {
            faceCam.SetActive(true);
        }

        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        state = State.Move;
    }

    // �ִϸ��̼� ������ bool��
    bool moveBool;

    void Update()
    {
        if (photonView.IsMine)
        {
            if (h + v == 0)
            {
                moveBool = false;
            }
            else moveBool = true;

            switch (state)
            {
                case State.Move:
                    //anim.SetBool("Move", moveBool);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Move", moveBool);
                    if (GameObject.Find("CreateRoomSet") != null || GameObject.Find("RoomList") != null)
                    {
                        return;
                    }
                        PlayerMove();
                    break;
                case State.Sit:
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Sit", true);
                    break;
                case State.Idle:
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Sit", false);
                    state = State.Move;
                    break;

            }
        }
    }

    Vector3 dir;
    float h = 0;
    float v = 0;

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
            yVelocity = 0;

        // �����̽��ٸ� ������ yVelocity�� jumpPower�� ����
        if (Input.GetButtonDown("Jump"))
            yVelocity = jumpPower;


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
        NK_TeacherManager.instance.JoinRoom();
    }
}