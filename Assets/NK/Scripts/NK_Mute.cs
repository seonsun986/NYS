using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NK_Mute : MonoBehaviour
{
    public Image muteImage;
    public Sprite muteSprite;
    public Sprite voiceSprite;

    private void Start()
    {
        muteImage.sprite = voiceSprite;
        // �л� ����Ʈ ���
        if (transform.GetComponentInChildren<Text>() != null)
        {
            for (int i = 0; i < GameManager.Instance.children.Count; i++)
            {
                if (GameManager.Instance.children[i].Owner.NickName == transform.GetComponentInChildren<Text>().text)
                {
                    AudioSource audio = GameManager.Instance.children[i].transform.Find("Speaker").GetComponent<AudioSource>();
                    if (audio.mute)
                    {
                        muteImage.sprite = muteSprite;
                    }
                    else
                    {
                        muteImage.sprite = voiceSprite;
                    }
                }
            }
        }
    }

    private void Update()
    {
        // ������ �ִ� ��ü ��Ʈ ��ư
        if (transform.GetComponentInChildren<Text>() == null)
        {
            ChangeImage();
        }
    }

    // ���Ұ� ���ο� ���� ��ư �̹��� ����
    public void ChangeImage()
    {
        if (NK_UIController.instance.IsMute)
        {
            muteImage.sprite = muteSprite;
        }
        else
        {
            muteImage.sprite = voiceSprite;
        }
    }

    // �̹����� ���� ��ư ��ư �̹��� ���� - ���̵� ���� �� ������ ����
    public void ChangeSingleImage()
    {
        if (muteImage.sprite == voiceSprite)
        {
            muteImage.sprite = muteSprite;
            NK_UIController.instance.ClickMute(true, transform.GetComponentInChildren<Text>().text);
        }
        else
        {
            muteImage.sprite = voiceSprite;
            NK_UIController.instance.ClickMute(false, transform.GetComponentInChildren<Text>().text);
        }
    }
}
