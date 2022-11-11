using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_MainCamera : MonoBehaviour
{
    public GameObject fairyTaleManager;
    GameObject player;
    Vector3 fairyTalePosition = new Vector3(0, 0.5f, -10);
    void Start()
    {

    }

    void Update()
    {
        if (fairyTaleManager.activeSelf)
        {
            transform.position = Vector3.Lerp(transform.position, fairyTalePosition, Time.deltaTime * 2);
            transform.rotation = Quaternion.identity;
            return;
        }

        if (player == null)
        {
            player = GameObject.Find("Player(Clone)");
            transform.localRotation = Quaternion.Euler(30, 0, 0);
        }

        else if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0.045f, 2.54f, -2.8f), Time.deltaTime * 200);
            transform.localRotation = Quaternion.Euler(30, 0, 0);
        }
    }
}
