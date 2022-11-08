using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NK_SettingUI : MonoBehaviourPun
{
    public Slider slider;
    // �ڽŸ��� Speaker�� ��ȯ
    public GameObject Speaker;
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
        // �����̴� �� ����Ǹ� ȣ���
        foreach (PhotonView child in GameManager.Instance.children)
        {
            Speaker = child.gameObject;
            audioSource = Speaker.transform.GetChild(2).GetComponent<AudioSource>();
            audioSource.volume = slider.value;
        }
    }
}
