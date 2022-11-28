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
        // ���̳ʽ� ��ư ������ ��
        slider.value -= 0.1f;
    }

    public void ClickPlus()
    {
        // �÷��� ��ư ������ ��
        slider.value += 0.1f;
    }

    public void ChangeSliderValue()
    {
        stageAudio.volume = slider.value;
        fairyTaleAudio.volume = slider.value;
        GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speaker");
        // �����̴� �� ����Ǹ� ȣ���
        foreach (GameObject speaker in speakers)
        {
            audioSource = speaker.GetComponent<AudioSource>();
            audioSource.volume = slider.value;
        }
    }
}
