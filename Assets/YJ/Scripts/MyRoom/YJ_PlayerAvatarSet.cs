using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


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

    //Test

    void Start()
    {

        // ���� �濡 �������ִ»��
        playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

        if (photonView.IsMine)
        {
            photonView.RPC("RpcAvtSet", RpcTarget.All, (int.Parse(UserInfo.animal)), (int.Parse(UserInfo.material)), (int.Parse(UserInfo.objectName)), UserInfo.memberRole == "TEACHER");
        }
    }

    int playerIndex;
    void Update()
    {
        // ���ο� ����� �������� RPC �ٽ� ���ֱ�
        if (SceneManager.GetActiveScene().name != "MyRoomScene")
        {

            if (playerIndex != PhotonNetwork.CurrentRoom.Players.Count)
            {
                // ��ĳ���� ���� ���
                if (photonView.IsMine)
                {
                    photonView.RPC("RpcAvtSet", RpcTarget.All, (int.Parse(UserInfo.animal)), (int.Parse(UserInfo.material)), (int.Parse(UserInfo.objectName)), UserInfo.memberRole == "TEACHER");
                }

                playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
            }
        }
    }

    // RPC �Ѱ��ֱ�
    [PunRPC]
    public void RpcAvtSet(int avtNum, int matNum, int objNum, bool role)
    {
        avt = avatar[avtNum];
        avt.SetActive(true);


        if (avtNum == 0)
        {
            avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = catMt[matNum];
        }
        else if (avtNum == 1)
        {
            avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = bearMt[matNum];
        }
        else if (avtNum == 2)
        {
            avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = bunnyMt[matNum];
        }

        // obj ��ư
        GameObject bag; // obj�� �� �� (���)
        GameObject rightHand; // obj�� �� �� (������)
        GameObject head; // obj�� �� �� (����)
        GameObject eyes; // obj�� �� �� (�Ȱ�)

        // ����
        GameObject minibag0;
        GameObject minibag1;

        // ����
        GameObject fish0;
        GameObject fish1;

        // ����
        GameObject miniHat0;
        GameObject miniHat1;

        // �Ȱ�
        GameObject glass0;
        GameObject glass1;

        int obj_1 = 8;
        int obj_2 = 8;
        int obj_3 = 8;
        int obj_4 = 8;

        string s = objNum.ToString();
        if (s.Length == 1)
        {
            obj_1 = int.Parse(s.Substring(0, 1));
        }
        else if (s.Length == 2)
        {
            obj_2 = int.Parse(s.Substring(1, 1));
        }
        else if (s.Length == 3)
        {
            obj_3 = int.Parse(s.Substring(3, 1));
        }
        else if (s.Length == 4)
        {
            obj_4 = int.Parse(s.Substring(4, 1));
        }

        //if(���� != 8)
        //{
        //    Instantiate(obj[����].gameObject, bag.transform);
        //}

        //if (���� != 8)
        //{
        //    Instantiate(obj[����+2].gameObject, bag.transform);
        //}

        //if (���� != 8)
        //{
        //    Instantiate(obj[���� + 4].gameObject, bag.transform);
        //}

        if (obj_1 < 2 || obj_2 <2 || obj_3 < 2 || obj_4 < 2)
        {
            bag = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
            if (obj_1 == 0 || obj_2 == 0 || obj_3 == 0 || obj_4 == 0)
            {
                minibag0 = Instantiate(obj[0].gameObject, bag.transform);
            }
            else if (obj_1 == 1 || obj_2 == 1 || obj_3 == 1 || obj_4 == 1)
            {
                minibag1 = Instantiate(obj[objNum].gameObject, bag.transform);
            }
        }
        else if (obj_1 > 1 && obj_1 < 4 || obj_2 > 1 && obj_2 < 4 || obj_3 > 1 && obj_3 < 4 || obj_4 > 1 && obj_4 < 4)
        {
            rightHand = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(4).gameObject;

            if (obj_1 == 2 || obj_2 == 2 || obj_3 == 2 || obj_4 == 2)
            {
                fish0 = Instantiate(obj[objNum].gameObject, rightHand.transform);
            }
            else if (obj_1 == 3 || obj_2 == 3 || obj_3 == 3 || obj_4 == 3)
            {
                fish1 = Instantiate(obj[objNum].gameObject, rightHand.transform);
            }
        }
        else if (obj_1 > 3 && obj_1 < 6 || obj_2 > 3 && obj_2 < 6 || obj_3 > 3 && obj_3 < 6 || obj_4 > 3 && obj_4 < 6)
        {
            head = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;

            if (obj_1 == 4 || obj_2 == 4 || obj_3 == 4 || obj_4 == 4)
            {
                miniHat0 = Instantiate(obj[objNum].gameObject, head.transform);
            }
            else if (obj_1 == 5 || obj_2 == 5 || obj_3 == 5 || obj_4 == 5)
            {
                miniHat1 = Instantiate(obj[objNum].gameObject, head.transform);
            }
        }
        else if (obj_1 > 5 && obj_1 < 8 || obj_2 > 5 && obj_2 < 8 || obj_3 > 5 && obj_3 < 8 || obj_4 > 5 && obj_4 < 8)
        {
            eyes = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;

            if (obj_1 == 6 || obj_2 == 6 || obj_3 == 6 || obj_4 == 6)
            {
                glass0 = Instantiate(obj[objNum].gameObject, eyes.transform);
            }
            else if (obj_1 == 7 || obj_2 == 7 || obj_3 == 7 || obj_4 == 7)
            {
                glass1 = Instantiate(obj[objNum].gameObject, eyes.transform);
            }
        }


        if (role)
        {
            crown = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject;
            crown.SetActive(true);
        }
    }
}
