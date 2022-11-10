using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;


// ĳ���� ������ �޾ƿͼ�
// �����Ҷ� �����ϰ�ʹ�.
public class YJ_PlayerAvatarSet : YJ_AvatarSet
{
    public override void Start()
    {
        base.Start();
        
        if (photonView.IsMine)
        {   // ���� �濡 �������ִ»��
            playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

            userInfo = YJ_DataManager.instance.myInfo;
            GetComponent<NK_PlayerMove>().SetAnim(int.Parse(userInfo.animal));
            AvtSet();
            photonView.RPC("RpcMemberId", RpcTarget.All, userInfo.memberCode);

        }
    }

    int playerIndex;
    void Update()
    {
        // ���ο� ����� �������� RPC �ٽ� ���ֱ�
        if (SceneManager.GetActiveScene().name != "MyRoomScene" && userInfo.memberCode != null)
        {
            if (playerIndex != PhotonNetwork.CurrentRoom.Players.Count)
            {
                // ��ĳ���� ���� ���
                if (photonView.IsMine)
                {
                    photonView.RPC("RpcMemberId", RpcTarget.All, userInfo.memberCode);
                    playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
                }
            }
        }
    }

    [PunRPC]
    void RpcMemberId(string memberId)
    {
        if (userInfo.memberCode != null && userInfo.memberCode.Length > 0) return;

        userInfo.memberCode = memberId;

        //�������� ��û
        Login_2_API();
    }

    public void Login_2_API()
    {
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/members/" + userInfo.memberCode;
        requester.requestType = RequestType.GET;
        //requester.headers = new Dictionary<string, string>();
        //requester.headers["accesstoken"] = userInfo.accessToken;
        
        requester.onComplete = (handler) => {

            JObject jsonData = JObject.Parse(handler.downloadHandler.text);

            userInfo.animal = jsonData["data"]["avatar"]["animal"].ToString();
            userInfo.material = jsonData["data"]["avatar"]["material"].ToString();
            userInfo.objectName = jsonData["data"]["avatar"]["objectName"].ToString();
            userInfo.nickname = jsonData["data"]["member"]["nickname"].ToString();
            userInfo.memberRole = jsonData["data"]["member"]["memberRole"].ToString();

            GetComponent<NK_PlayerMove>().SetAnim(int.Parse(userInfo.animal));
            AvtSet();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }      
}
