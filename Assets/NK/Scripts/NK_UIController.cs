using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_UIController : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region // ���Ұ� ��ư
    public void ClickMute()
    {
        // ��� ���̵��� ������ 0���� �ϰų� Mute ��Ŵ
        for(int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            AudioSource audio = GameManager.Instance.children[i].GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.mute = IsMute;
            }
        }
    }
    #endregion

    #region // �ൿ���� ��ư
    public void ClickControl()
    {
        // ��� ���̵��� ���� ����� �� �¼��� ����
        if(IsControl)
        {
            // ��� �¼��� ������
            // ���̵�� �¼��� �Ÿ��� ����
            // ���� ����� �� �¼��� ����
        }
    }
    #endregion
}
