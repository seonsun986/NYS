using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NK_Door : MonoBehaviourPun
{
    private static bool isOpen = false;
    public bool IsOpen
    {
        get
        {
            if (isOpen)
                isOpen = false;
            else
                isOpen = true;
            return isOpen;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickDoor();
        }
    }

    public void ClickDoor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 광선으로 충돌된 collider를 hit에 넣습니다.
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == gameObject)
            {
                photonView.RPC("RPCClickDoor", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void RPCClickDoor()
    {
        if (IsOpen)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2.5f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2.5f);
        }
    }
}
