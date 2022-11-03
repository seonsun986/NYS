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
        rb.AddForce(transform.forward * 5, ForceMode.Impulse);
    }
}
