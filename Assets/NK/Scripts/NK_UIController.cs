using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NK_UIController : MonoBehaviour
{
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
        // ��� ���̵��� ������ 0���� �ϰų� Mute ��Ŵ
        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            AudioSource audio = GameManager.Instance.children[i].GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.mute = IsMute;
            }
        }
    }
    #endregion

    #region ClickControl // �ൿ���� ��ư
    float shortDistance = float.MaxValue;
    GameObject nearSeat = null;
    public void ClickControl()
    {
        // ��� ���̵��� ���� ����� �� �¼��� ����
        if (IsControl)
        {
            // ��� �¼��� ������
            List<GameObject> seats = GameObject.FindGameObjectsWithTag("Seat").ToList<GameObject>();

            for (int i = 0; i < GameManager.Instance.children.Count; i++)
            {
                GameObject child = GameManager.Instance.children[i].gameObject;
                child.transform.forward = Vector3.forward;
                // �÷��̾� ���� ��ũ��Ʈ ��Ȱ��ȭ
                NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
                move.enabled = false;
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
                child.transform.position = new Vector3(nearSeat.transform.position.x, child.transform.position.y, nearSeat.transform.position.z);
                seats.Remove(nearSeat);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.children.Count; i++)
            {
                GameObject child = GameManager.Instance.children[i].gameObject;
                // �÷��̾� ���� ��ũ��Ʈ Ȱ��ȭ
                NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
                move.enabled = true;
            }
        }
    }
    #endregion
}