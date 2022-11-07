using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NK_SettingUI : MonoBehaviourPun
{
    public Slider slider;
    // 자신만의 Speaker로 변환
    public GameObject Speaker;
    AudioSource audioSource;

    public void ClickMinus()
    {
        // 마이너스 버튼 눌렀을 때
        slider.value -= 0.1f;
    }

    public void ClickPlus()
    {
        // 플러스 버튼 눌렀을 때
        slider.value += 0.1f;
    }

    public void ChangeSliderValue()
    {
        // 슬라이더 값 변경되면 호출됨
        foreach (PhotonView child in GameManager.Instance.children)
        {
            Speaker = child.gameObject;
            audioSource = Speaker.transform.GetChild(2).GetComponent<AudioSource>();
            audioSource.volume = slider.value;
        }
    }
}
