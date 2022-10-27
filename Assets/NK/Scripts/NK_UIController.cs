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

    #region ClickMute // ���Ұ� ��ư
    public void ClickMute()
    {
        photonView.RPC("RPCMute", RpcTarget.All);
    }

    [PunRPC]
    private void RPCMute()
    {
        // ��� ���̵��� ������ 0���� �ϰų� Mute ��Ŵ
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
                //child.transform.forward = Vector3.forward;
                // Sit �ִϸ��̼� ����
                //NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
                //move.state = NK_PlayerMove.State.Sit;
                //move.enabled = false;
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
                print(nearSeat);
                photonView.RPC("RpcControl", RpcTarget.All, child.GetPhotonView().ViewID, nearSeat.transform.position);
                // ���� ����� �� �¼��� ����
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
        /*        // ��� �¼��� ������
                seats = GameObject.FindGameObjectsWithTag("Seat").ToList<GameObject>();

                for (int i = 0; i < GameManager.Instance.children.Count; i++)
                {
                    GameObject child = GameManager.Instance.children[i].gameObject;
                    child.transform.forward = Vector3.forward;
                    // Sit �ִϸ��̼� ����
                    NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
                    move.state = NK_PlayerMove.State.Sit;
                    //move.enabled = false;
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
                    // ���� ����� �� �¼��� ����
                    child.transform.position = nearSeat.transform.position;
                    seats.Remove(nearSeat);
                }*/
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
}