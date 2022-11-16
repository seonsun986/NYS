using System.Collections;
using System.Collections.Generic;
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
        ChangeImage();
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
