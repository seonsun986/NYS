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

    #region IsSelectThema
    bool isSelectThema = false;
    bool IsSelectThema
    {
        get
        {
            if (isSelectThema)
                isSelectThema = false;
            else
                isSelectThema = true;
            return isSelectThema;
        }
    }
    #endregion
    
    #region IsSelectBook
    bool isSelectBook = false;
    bool IsSelectBook
    {
        get
        {
            if (isSelectBook)
                isSelectBook = false;
            else
                isSelectBook = true;
            return isSelectBook;
        }
    }
    #endregion

    #region IsSetting
    bool isSetting = false;
    bool IsSetting
    {
        get
        {
            if (isSetting)
                isSetting = false;
            else
                isSetting = true;
            return isSetting;
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
                GameObject child = GameManager.Instance.children[i];
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
                GameObject child = GameManager.Instance.children[i];
                // �÷��̾� ���� ��ũ��Ʈ Ȱ��ȭ
                NK_PlayerMove move = child.GetComponent<NK_PlayerMove>();
                move.enabled = true;
            }
        }
    }
    #endregion

    #region ClickSelectThema // �� �׸� ����
    public GameObject ThemaUI;
    public void ClickSelectThema()
    {
        ThemaUI.SetActive(IsSelectThema);
    }
    #endregion
    
    #region ClickSelectBook // ��ȭå ����
    public GameObject BookUI;
    public void ClickSelectBook()
    {
        BookUI.SetActive(IsSelectBook);
    }
    #endregion

    #region ClickManagement // �л� ����
    public void ClickManagement()
    {

    }
    #endregion

    #region ClickSetting // ���� ����
    public GameObject SettingUI;
    public void ClickSetting()
    {
        SettingUI.SetActive(IsSetting);
    }
    #endregion

    #region ClickBack // �������� ���ư���
    public void ClickBack()
    {

    }
    #endregion
}