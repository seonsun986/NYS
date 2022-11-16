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

    // 이미지에 따른 버튼 버튼 이미지 변경 - 아이들 관리 탭 누르면 실행
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
