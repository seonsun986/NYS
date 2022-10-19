using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class YJ_MainCamera : MonoBehaviourPun
{

    GameObject player;
    void Start()
    {

    }

    void Update()
    {

        if (player == null)
        {
            player = GameObject.Find("Cube(Clone)");
        }

        else if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0.045f, 2.54f, -2.8f), Time.deltaTime * 200);
        }
    }
}
