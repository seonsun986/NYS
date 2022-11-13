using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Animation : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void BearCarrot()
    {
        Rigidbody rb = gameObject.transform.GetChild(1).GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(transform.forward * 18, ForceMode.Impulse);
    }

    public void TigerCarrot()
    {
        Rigidbody rb = gameObject.transform.GetChild(1).GetComponent<Rigidbody>();
        rb.useGravity = true;
        //rb.AddForce(transform.right * -1 * 18, ForceMode.Impulse);
        rb.AddForce(transform.forward * 10, ForceMode.Impulse);

    }

    public void SoccerBall()
    {
        GameObject ball = gameObject.transform.GetChild(2).gameObject;
        ball.transform.localPosition = new Vector3(0.13f, 0.02f, 0.39f);
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 speed = new Vector3(-2000, 300, 0);
        Rigidbody rb = gameObject.transform.GetChild(2).GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(speed);
    }

    public void BulbOn()
    {
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void Test()
    {
        print("¹Ùº¸");
    }

}
