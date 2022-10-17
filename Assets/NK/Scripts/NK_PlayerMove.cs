using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_PlayerMove : MonoBehaviour
{
    // �ӷ�
    public float moveSpeed = 5;
    // ���� �Ŀ�
    public float jumpPower = 5;

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
    }

    // Update is called once per frame
    void Update()
    {
        // 1. WSAD�� ��ȣ�� ����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. ���� ��ȣ�� ������ �����
        Vector3 dir = transform.forward * v + transform.right * h;

        dir.Normalize();

        // ���࿡ �ٴڿ� ����ִٸ� yVelocity�� 0���� ����
        //if (controller.isGrounded)
            //yVelocity = 0;

        // �����̽��ٸ� ������ yVelocity�� jumpPower�� ����
        if (Input.GetButtonDown("Jump"))
            yVelocity = jumpPower;

        // yVelocity���� �߷����� ���ҽ�Ŵ
        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        controller.Move(dir * moveSpeed * Time.deltaTime);
    }
}
