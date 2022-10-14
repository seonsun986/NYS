using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Move : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpPower = 5;

    CharacterController controller;
    float yVelocity;
    float gravity = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = transform.forward * v + transform.right * h;

        dir.Normalize();

        if (controller.isGrounded)
            yVelocity = 0;

        if (Input.GetButtonDown("Jump"))
            yVelocity = jumpPower;

        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        controller.Move(dir * moveSpeed * Time.deltaTime);
    }
}
