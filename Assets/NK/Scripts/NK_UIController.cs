using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NK_UIController : MonoBehaviourPun
{
    public static NK_UIController instance;
    List<GameObject> seats;

    #region IsMute
    bool isMute = false;
    public bool IsMute
    {
        get
        {
            return isMute;
        }
        set
        {
            isMute = value;
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

    bool isHandUp = false;
    public bool IsHandUp
    {
        get
        {
            if (isHandUp)
                isHandUp = false;
            else
                isHandUp = true;
            return isHandUp;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    #region ClickMute // 음소거 버튼
    public void ClickMute()
    {
        if (IsMute)
            IsMute = false;
        else
            IsMute = true;
        photonView.RPC("RPCMute", RpcTarget.All, IsMute);
    }

    [PunRPC]
    private void RPCMute(bool mute)
    {
        // 모든 아이들을 Mute 시킴
        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            AudioSource audio = GameManager.Instance.children[i].transform.Find("Speaker").GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.mute = mute;
            }
        }
    }

    public void ClickMute(bool mute, string nickname)
    {
        // mute 상태 풀릴 때
        if (!mute)
        {
            // mute = false
            IsMute = mute;
        }
        else
        {
            int muteCount = 0;
            for (int i = 0; i < GameManager.Instance.children.Count; i++)
            {
                if (GameManager.Instance.children[i].Owner.NickName != nickname)
                {
                    // 전체 아이들의 mute 상태 체크함
                    if (GameManager.Instance.children[i].transform.Find("Speaker").GetComponent<AudioSource>().mute)
                    {
                        muteCount++;
                    }
                }
            }
            // 전체 아이들이 mute 상태라면
            if (muteCount == GameManager.Instance.children.Count - 1)
            {
                IsMute = mute;
            }
        }

        photonView.RPC("RPCSingleMute", RpcTarget.All, mute, nickname);
    }

    [PunRPC]
    private void RPCSingleMute(bool mute, string nickname)
    {
        // 멤버 관리에서 특정 아이를 Mute 시킴
        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            if (GameManager.Instance.children[i].Owner.NickName == nickname)
            {
                AudioSource audio = GameManager.Instance.children[i].transform.Find("Speaker").GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.mute = mute;
                }
            }
        }
    }
    #endregion

    public void ClickHandDown(string nickname)
    {
        photonView.RPC("RPCHandDown", RpcTarget.All, nickname);
    }

    [PunRPC]
    private void RPCHandDown(string nickname)
    {
        GameObject child = null;
        GameObject[] hands = GameObject.FindGameObjectsWithTag("Hand");
        // 특정 아이를 손을 내리게 함
        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            if (GameManager.Instance.children[i].Owner.NickName == nickname)
            {
                child = GameManager.Instance.children[i].gameObject;
                // HandUp 애니메이션 설정
                NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
                move.state = NK_PlayerMove.State.Sit;
            }
        }

        for (int j = 0; j < hands.Length; j++)
        {
            if (child != null && hands[j].transform.position.x == child.transform.position.x)
            {
                Destroy(hands[j]);
            }
        }
    }

    #region ClickControl // 행동제어 버튼
    float shortDistance = float.MaxValue;
    GameObject nearSeat = null;
    // 버튼 클릭 시 행동 제어
    public void ClickControl()
    {
        // 모든 아이들을 가장 가까운 빈 좌석에 앉힘
        if (IsControl)
        {
            ControlChildren();
        }
        else
        {
            photonView.RPC("RpcEndControl", RpcTarget.All);
        }
    }

    private void ControlChildren()
    {
        // 모든 좌석을 가져옴
        seats = GameObject.FindGameObjectsWithTag("Seat").ToList<GameObject>();

        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            GameObject child = GameManager.Instance.children[i].gameObject;
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

            photonView.RPC("RpcControl", RpcTarget.All, child.GetPhotonView().ViewID, nearSeat.transform.position);
            seats.Remove(nearSeat);
        }
    }

    // 동화책 펼칠 때 행동제어
    public Vector3 deskPosition;
    public Vector3 deskRotation;
    public void ClickControl(bool isControl)
    {
        GameObject teacher = GameObject.FindGameObjectWithTag("Teacher");
        // 모든 아이들을 가장 가까운 빈 좌석에 앉힘
        if (isControl)
        {
            ControlChildren();
            if (teacher != null)
            {
                // 선생님 위치 조절
                photonView.RPC("RpcTeacherControl", RpcTarget.All, teacher.GetPhotonView().ViewID, deskPosition, deskRotation);
            }
        }
        else
        {
            photonView.RPC("RpcEndControl", RpcTarget.All);
            if (teacher != null)
            {
                photonView.RPC("RpcEndTeacherControl", RpcTarget.All, teacher.GetPhotonView().ViewID);
            }
        }
    }

    // 선생님 RPC
    [PunRPC]
    private void RpcTeacherControl(int viewId, Vector3 deskPos, Vector3 deskRot)
    {
        GameObject teacher = PhotonView.Find(viewId).gameObject;
        NK_PlayerMove move = teacher.GetComponent<NK_PlayerMove>();
        move.anim.SetBool("Move", false);
        move.enabled = false;
        teacher.transform.localEulerAngles = deskRot;
        teacher.transform.position = deskPos;
    }

    [PunRPC]
    private void RpcEndTeacherControl(int viewId)
    {
        GameObject teacher = PhotonView.Find(viewId).gameObject;
        NK_PlayerMove move = teacher.GetComponent<NK_PlayerMove>();
        move.enabled = true;
        move.sit = true;
        move.state = NK_PlayerMove.State.Idle;
    }

    [PunRPC]
    private void RpcControl(int viewId, Vector3 seatPos)
    {
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

    GameObject hand;

    public void ClickHandUp()
    {
        if (GameManager.Instance.photonView.IsMine && IsHandUp)
        {
            photonView.RPC("RpcHandUp", RpcTarget.All, GameManager.Instance.photonView.ViewID);
            hand = PhotonNetwork.Instantiate("Hand", GameManager.Instance.photonView.transform.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
        }
        else
        {
            photonView.RPC("RpcEndHandUp", RpcTarget.All, GameManager.Instance.photonView.ViewID);
            PhotonNetwork.Destroy(hand);
        }
    }

    public void HandDown()
    {
        if (GameManager.Instance.photonView.IsMine && isHandUp)
        {
            PhotonNetwork.Destroy(hand);
        }
    }

    [PunRPC]
    private void RpcHandUp(int viewId)
    {
        GameObject child = PhotonView.Find(viewId).gameObject;
        // HandUp 애니메이션 설정
        NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
        move.state = NK_PlayerMove.State.HandUp;
    }

    [PunRPC]
    private void RpcEndHandUp(int viewId)
    {
        GameObject child = PhotonView.Find(viewId).gameObject;
        // HandUp 애니메이션 설정
        NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
        move.state = NK_PlayerMove.State.Sit;
    }
}