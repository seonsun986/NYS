using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NK_ManageUI : MonoBehaviourPun
{
    public GameObject childListFactory;
    public Transform childrenListContent;

    void OnEnable()
    {
        foreach (PhotonView child in GameManager.Instance.children)
        {
            Text nickname = childListFactory.transform.GetChild(0).GetComponent<Text>();
            nickname.text = child.Owner.NickName;
            GameObject childList = GameObject.Instantiate(childListFactory);
            childList.transform.parent = childrenListContent;
            print(child.Owner.NickName);
        }
    }
}
