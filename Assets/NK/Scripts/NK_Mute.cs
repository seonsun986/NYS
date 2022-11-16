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

    // 음소거 여부에 따른 버튼 이미지 변경
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

    public void ChangeSingleImage()
    {
        if (muteImage.sprite == voiceSprite)
        {
            muteImage.sprite = muteSprite;
            NK_UIController.instance.ClickMute(transform.GetComponentInChildren<Text>().text);
        }
        else
        {
            muteImage.sprite = voiceSprite;
        }
    }
}
