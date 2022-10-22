using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_ManageUI : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //print(pv.Owner.UserId);
        foreach (PhotonView child in GameManager.Instance.children)
        {
            print(child.Owner.UserId);
            //PhotonNetwork.
        }

    }


}
