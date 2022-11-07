using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

// 내가 만약 프라자씬의 방장이면
// ismine인 오브젝트를 모아서
// 내 캐릭터가 아닌것을 삭제할거다
public class YJ_IfMaster : MonoBehaviourPun
{
    int viewID;
    int playerIndex = 0;
    public List<GameObject> ismine = new List<GameObject>();
    void Start()
    {


        // 광장에있는 사람 수
        if (SceneManager.GetActiveScene().name == "PlazaScene" && PhotonNetwork.IsMasterClient)
        {
            playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
        }
        else
            playerIndex = 0;

        // 내 뷰아이디 저장
        viewID = GetComponent<PhotonView>().ViewID;
        
    }    
}
