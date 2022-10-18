using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_PlayerMove : MonoBehaviour
{
    // 속력
    public float moveSpeed = 5;
    // 점프 파워
    public float jumpPower = 5;

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
    }

    // Update is called once per frame
    void Update()
    {
        // 1. WSAD의 신호를 받자
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. 받은 신호로 방향을 만든다
        Vector3 dir = transform.forward * v + transform.right * h;

        dir.Normalize();

        // 만약에 바닥에 닿아있다면 yVelocity를 0으로 하자
        //if (controller.isGrounded)
            //yVelocity = 0;

        // 스페이스바를 누르면 yVelocity에 jumpPower를 셋팅
        if (Input.GetButtonDown("Jump"))
            yVelocity = jumpPower;

        // yVelocity값을 중력으로 감소시킴
        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        controller.Move(dir * moveSpeed * Time.deltaTime);
    }
}
