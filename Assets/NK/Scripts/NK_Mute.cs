using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NK_Mute : MonoBehaviour
{
    public Image muteImage;
    public Sprite muteSprite;
    public Sprite voiceSprite;

    private void Start()
    {
        muteImage.sprite = voiceSprite;
    }

    // ���Ұ� ���ο� ���� ��ư �̹��� ����
    public void ChangeImage()
    {
        if (muteImage.sprite == voiceSprite)
        {
            muteImage.sprite = muteSprite;
        }
        else
        {
            muteImage.sprite = voiceSprite;
        }
    }
}
