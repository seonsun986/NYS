using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NK_Like : MonoBehaviourPun
{
    public GameObject likeFactory;
    public Text likeCount;
    int count;

    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;
            likeCount.text = value.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        likeCount = GameObject.Find("LikeCount").GetComponent<Text>();
        likeCount.text = "0";
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickLike()
    {
        if (GameManager.Instance.photonView.IsMine)
        {
            photonView.RPC("RPCClickLike", RpcTarget.All);
            // 하트 애니메이션 뜨기
            GameObject child = GameManager.Instance.photonView.gameObject;
            PhotonNetwork.Instantiate("NK/" + likeFactory.name, child.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        }
    }

    [PunRPC]
    public void RPCClickLike()
    {
        // 좋아요 개수가 하나 늘고
        Count++;
    }
}
