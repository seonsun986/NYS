using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

// ���� ���� �����ھ��� �����̸�
// ismine�� ������Ʈ�� ��Ƽ�
// �� ĳ���Ͱ� �ƴѰ��� �����ҰŴ�
public class YJ_IfMaster : MonoBehaviourPun
{
    int viewID;
    int playerIndex = 0;
    public List<GameObject> ismine = new List<GameObject>();
    void Start()
    {


        // ���忡�ִ� ��� ��
        if (SceneManager.GetActiveScene().name == "PlazaScene" && PhotonNetwork.IsMasterClient)
        {
            playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
        }
        else
            playerIndex = 0;

        // �� ����̵� ����
        viewID = GetComponent<PhotonView>().ViewID;
        
    }    
}
