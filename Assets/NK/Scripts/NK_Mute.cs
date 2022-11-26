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
        // 학생 리스트 라면
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
        // 상위에 있는 전체 뮤트 버튼
        if (transform.GetComponentInChildren<Text>() == null)
        {
            ChangeImage();
        }
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
