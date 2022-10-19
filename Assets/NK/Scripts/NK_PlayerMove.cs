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

    // �ִϸ��̼�
    Animator anim;

    // �ӷ�
    public float moveSpeed = 3;
    // ���� �Ŀ�
    public float jumpPower = 3;

    CharacterController controller;
    // y���� �ӷ�
    float yVelocity;
    // �߷�
    float gravity = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.AddPlayer(gameObject);
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        state = State.Idle;
    }

    // �ִϸ��̼� ������ bool��
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
        //    // Lerp�� �̿��ؼ� ������, ����������� �̵� �� ȸ��
        //    transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        //}
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

}