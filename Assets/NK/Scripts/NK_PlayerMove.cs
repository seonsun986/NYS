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
    }
    
    State state;

    // 애니메이션
    Animator anim;

    // 속력
    public float moveSpeed = 3;
    // 점프 파워
    public float jumpPower = 3;

    CharacterController controller;
    // y방향 속력
    float yVelocity;
    // 중력
    float gravity = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.AddPlayer(gameObject);
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        state = State.Idle;
    }

    // 애니메이션 조절할 bool값
    bool moveBool;

    void Update()
    {
        if(h + v == 0)
        {
            moveBool = false;
        }
        else moveBool = true;


        //if (photonView.IsMine)
        {

            switch (state)
            {
                case State.Idle:
                    anim.SetBool("Move", moveBool);
                    PlayerMove();
                    break;

                case State.Move:
                    break;

            }
        }
        //else
        //{
        //    // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
        //    transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        //}
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

}