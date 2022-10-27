using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NK_UIController : MonoBehaviourPun
{
    public List<GameObject> seats;

    #region IsMute
    bool isMute = false;
    bool IsMute
    {
        get
        {
            if (isMute)
                isMute = false;
            else
                isMute = true;
            return isMute;
        }
    }
    #endregion

    #region IsControl
    bool isControl = false;
    bool IsControl
    {
        get
        {
            if (isControl)
                isControl = false;
            else
                isControl = true;
            return isControl;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region ClickMute // 음소거 버튼
    public void ClickMute()
    {
        photonView.RPC("RPCMute", RpcTarget.All);
    }

    [PunRPC]
    private void RPCMute()
    {
        // 모든 아이들의 볼륨을 0으로 하거나 Mute 시킴
        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            AudioSource audio = GameManager.Instance.children[i].transform.GetChild(2).GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.mute = IsMute;
            }
        }
    }
    #endregion

    #region ClickControl // 행동제어 버튼
    float shortDistance = float.MaxValue;
    public GameObject nearSeat = null;
    public void ClickControl()
    {
        // 모든 아이들을 가장 가까운 빈 좌석에 앉힘
        if (IsControl)
        {
            // 모든 좌석을 가져옴
            seats = GameObject.FindGameObjectsWithTag("Seat").ToList<GameObject>();

            for (int i = 0; i < GameManager.Instance.children.Count; i++)
            {
                GameObject child = GameManager.Instance.children[i].gameObject;
                //child.transform.forward = Vector3.forward;
                // Sit 애니메이션 설정
                //NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
                //move.state = NK_PlayerMove.State.Sit;
                //move.enabled = false;
                // 가장 짧은 거리 초기화
                shortDistance = float.MaxValue;

                for (int j = 0; j < seats.Count; j++)
                {
                    float distance = Vector3.Distance(seats[j].transform.position, child.transform.position);
                    // 아이들과 좌석의 거리를 비교함
                    if (distance < shortDistance)
                    {
                        shortDistance = distance;
                        nearSeat = seats[j];
                    }
                }
                print(nearSeat);
                photonView.RPC("RpcControl", RpcTarget.All, child.GetPhotonView().ViewID, nearSeat.transform.position);
                // 가장 가까운 빈 좌석에 앉힘
                //child.transform.position = nearSeat.transform.position;
                seats.Remove(nearSeat);
            }

            //photonView.RPC("RpcControl", RpcTarget.All);
        }
        else
        {
            photonView.RPC("RpcEndControl", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RpcControl(int viewId, Vector3 seatPos)
    {
        /*        // 모든 좌석을 가져옴
                seats = GameObject.FindGameObjectsWithTag("Seat").ToList<GameObject>();

                for (int i = 0; i < GameManager.Instance.children.Count; i++)
                {
                    GameObject child = GameManager.Instance.children[i].gameObject;
                    child.transform.forward = Vector3.forward;
                    // Sit 애니메이션 설정
                    NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
                    move.state = NK_PlayerMove.State.Sit;
                    //move.enabled = false;
                    // 가장 짧은 거리 초기화
                    shortDistance = float.MaxValue;

                    for (int j = 0; j < seats.Count; j++)
                    {
                        float distance = Vector3.Distance(seats[j].transform.position, child.transform.position);
                        // 아이들과 좌석의 거리를 비교함
                        if (distance < shortDistance)
                        {
                            shortDistance = distance;
                            nearSeat = seats[j];
                        }
                    }
                    // 가장 가까운 빈 좌석에 앉힘
                    child.transform.position = nearSeat.transform.position;
                    seats.Remove(nearSeat);
                }*/
        GameObject child = PhotonView.Find(viewId).gameObject;
        child.transform.forward = Vector3.forward;
        // Sit 애니메이션 설정
        NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
        move.state = NK_PlayerMove.State.Sit;
        // 가장 가까운 빈 좌석에 앉힘
        child.transform.position = seatPos;
    }

    [PunRPC]
    private void RpcEndControl()
    {
        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            GameObject child = GameManager.Instance.children[i].gameObject;
            // 플레이어 무브 스크립트 활성화
            NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
            //move.enabled = true;
            // Idle 상태 설정
            move.state = NK_PlayerMove.State.Idle;
        }
    }
    #endregion
}