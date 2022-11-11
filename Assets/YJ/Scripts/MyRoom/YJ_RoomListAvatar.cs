using Newtonsoft.Json.Linq;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_RoomListAvatar : YJ_AvatarSet
{
    //public UserInfo roomSetAvatarInfo = new UserInfo();

    YJ_RoomText roomText;

    int playerIndex;
    string code;

    public override void Start()
    {
        if (code == null)
        {
            base.Start();

            if (photonView.IsMine)
            {   // 현재 방에 입장해있는사람
                playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

                code = this.transform.GetComponentInParent<YJ_RoomText>().createRoomerCode;

                userInfo = YJ_DataManager.instance.myInfo;
                GetComponent<NK_PlayerMove>().SetAnim(int.Parse(userInfo.animal));
                photonView.RPC("RpcMemberId", RpcTarget.All, code);
                //AvtSet();

            }
            roomText = this.transform.GetParentComponent<YJ_RoomText>();


        }
    }
    [PunRPC]
    void RpcMemberId(string memberId)
    {
        if (userInfo.memberCode != null && code.Length > 0) return;

        userInfo.memberCode = memberId;

        code = memberId;
        //서버한테 요청
        Room_Avt_API();
    }

    public void Room_Avt_API()
    {
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/members/" + code;
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
