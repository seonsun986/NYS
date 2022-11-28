using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NK_SettingUI : MonoBehaviourPun
{
    public Slider slider;
    public AudioSource stageAudio;
    public AudioSource fairyTaleAudio;
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
        stageAudio.volume = slider.value;
        fairyTaleAudio.volume = slider.value;
        GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speaker");
        // 슬라이더 값 변경되면 호출됨
        foreach (GameObject speaker in speakers)
        {
            audioSource = speaker.GetComponent<AudioSource>();
            audioSource.volume = slider.value;
        }
    }
}
