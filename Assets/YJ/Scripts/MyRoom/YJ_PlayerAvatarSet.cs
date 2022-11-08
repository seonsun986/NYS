using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


// 캐릭터 정보를 받아와서
// 생성할때 세팅하고싶다.
public class YJ_PlayerAvatarSet : MonoBehaviourPun
{
    // 캐릭터 정보
    public GameObject[] avatar;

    // 머티리얼 정보
    public Material[] catMt;
    public Material[] bearMt;
    public Material[] bunnyMt;
    // 오브젝트 정보
    public GameObject[] obj;

    // 캐릭터
    GameObject avt;
    GameObject crown;

    void Start()
    {
        // 현재 방에 입장해있는사람
        playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

        if (photonView.IsMine)
        {
            photonView.RPC("RpcAvtSet", RpcTarget.All, (int.Parse(UserInfo.animal)), (int.Parse(UserInfo.material)), UserInfo.memberRole == "TEACHER");
        }
    }

    int playerIndex;
    void Update()
    {
        // 새로운 사람이 들어왔을때 RPC 다시 쏴주기
        if (playerIndex != PhotonNetwork.CurrentRoom.Players.Count)
        {
            // 내캐릭터 정보 쏘기
            if (photonView.IsMine)
            {
                photonView.RPC("RpcAvtSet", RpcTarget.All, (int.Parse(UserInfo.animal)), (int.Parse(UserInfo.material)), UserInfo.memberRole == "TEACHER");
            }

            playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
        }
    }

    // RPC 넘겨주기
    [PunRPC]
    public void RpcAvtSet(int avtNum, int matNum, bool role)
    {
        avt = avatar[avtNum];
        avt.SetActive(true);
        avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = catMt[matNum];
        if (role)
        {
            crown = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject;
            crown.SetActive(true);
        }
    }
}
