using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


// ĳ���� ������ �޾ƿͼ�
// �����Ҷ� �����ϰ�ʹ�.
public class YJ_PlayerAvatarSet : MonoBehaviourPun
{
    // ĳ���� ����
    public GameObject[] avatar;

    // ��Ƽ���� ����
    public Material[] catMt;
    public Material[] bearMt;
    public Material[] bunnyMt;
    // ������Ʈ ����
    public GameObject[] obj;

    // ĳ����
    GameObject avt;
    GameObject crown;

    void Start()
    {
        // ���� �濡 �������ִ»��
        playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

        if (photonView.IsMine)
        {
            photonView.RPC("RpcAvtSet", RpcTarget.All, (int.Parse(UserInfo.animal)), (int.Parse(UserInfo.material)), UserInfo.memberRole == "TEACHER");
        }
    }

    int playerIndex;
    void Update()
    {
        // ���ο� ����� �������� RPC �ٽ� ���ֱ�
        if (playerIndex != PhotonNetwork.CurrentRoom.Players.Count)
        {
            // ��ĳ���� ���� ���
            if (photonView.IsMine)
            {
                photonView.RPC("RpcAvtSet", RpcTarget.All, (int.Parse(UserInfo.animal)), (int.Parse(UserInfo.material)), UserInfo.memberRole == "TEACHER");
            }

            playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
        }
    }

    // RPC �Ѱ��ֱ�
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
