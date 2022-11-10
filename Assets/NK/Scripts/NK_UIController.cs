using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    bool isHandUp = false;
    bool IsHandUp
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

    #region ClickMute // ���Ұ� ��ư
    public void ClickMute()
    {
        photonView.RPC("RPCMute", RpcTarget.All, IsMute);
    }

    [PunRPC]
    private void RPCMute(bool mute)
    {
        // ��� ���̵��� ������ 0���� �ϰų� Mute ��Ŵ
        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            AudioSource audio = GameManager.Instance.children[i].transform.Find("Speaker").GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.mute = mute;
            }
        }
    }
    #endregion

    #region ClickControl // �ൿ���� ��ư
    float shortDistance = float.MaxValue;
    public GameObject nearSeat = null;
    public void ClickControl()
    {
        // ��� ���̵��� ���� ����� �� �¼��� ����
        if (IsControl)
        {
            // ��� �¼��� ������
            seats = GameObject.FindGameObjectsWithTag("Seat").ToList<GameObject>();

            for (int i = 0; i < GameManager.Instance.children.Count; i++)
            {
                GameObject child = GameManager.Instance.children[i].gameObject;
                // ���� ª�� �Ÿ� �ʱ�ȭ
                shortDistance = float.MaxValue;

                for (int j = 0; j < seats.Count; j++)
                {
                    float distance = Vector3.Distance(seats[j].transform.position, child.transform.position);
                    // ���̵�� �¼��� �Ÿ��� ����
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
        else
        {
            photonView.RPC("RpcEndControl", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RpcControl(int viewId, Vector3 seatPos)
    {
        GameObject child = PhotonView.Find(viewId).gameObject;
        child.transform.forward = Vector3.forward;
        // Sit �ִϸ��̼� ����
        NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
        move.state = NK_PlayerMove.State.Sit;
        // ���� ����� �� �¼��� ����
        child.transform.position = seatPos;
    }

    [PunRPC]
    private void RpcEndControl()
    {
        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            GameObject child = GameManager.Instance.children[i].gameObject;
            // �÷��̾� ���� ��ũ��Ʈ Ȱ��ȭ
            NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
            //move.enabled = true;
            // Idle ���� ����
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
            hand = PhotonNetwork.Instantiate("Hand", GameManager.Instance.photonView.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
        else
        {
            photonView.RPC("RpcEndHandUp", RpcTarget.All, GameManager.Instance.photonView.ViewID);
            PhotonNetwork.Destroy(hand);
        }
    }

    [PunRPC]
    private void RpcHandUp(int viewId)
    {
        GameObject child = PhotonView.Find(viewId).gameObject;
        // HandUp �ִϸ��̼� ����
        NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
        move.state = NK_PlayerMove.State.HandUp;
    }

    [PunRPC]
    private void RpcEndHandUp(int viewId)
    {
        GameObject child = PhotonView.Find(viewId).gameObject;
        // HandUp �ִϸ��̼� ����
        NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
        move.state = NK_PlayerMove.State.Sit;
    }
}