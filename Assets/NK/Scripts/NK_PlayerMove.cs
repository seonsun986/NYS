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

    // Start is called before the first frame update
    void Start()
    {

        if (photonView.IsMine)
        {
            faceCam.SetActive(true);
        }

        print(PhotonNetwork.MasterClient.NickName);
        // 아직 방에 들어갈 수 없어서 임시로 테스트 중...
        if (GameObject.Find("GameManager"))
        {
            if (PhotonNetwork.MasterClient.NickName != photonView.Owner.NickName)
            {
                gameObject.tag = "Child";
                if (photonView.IsMine)
                {
                    GameObject.Find("TeacherUI").SetActive(false);

                }
            }
            else
            {
                gameObject.tag = "Teacher";
            }
            GameManager.Instance.AddPlayer(photonView);

        }
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        state = State.Move;
    }

    // 애니메이션 조절할 bool값
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
            yVelocity = 0;

        // 스페이스바를 누르면 yVelocity에 jumpPower를 셋팅
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
}