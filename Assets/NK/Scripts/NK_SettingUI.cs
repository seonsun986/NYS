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
    // Start is called before the first frame update
    void Start()
    {
        Speaker = GameObject.FindWithTag("Teacher");
        audioSource = Speaker.transform.GetChild(2).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        audioSource.volume = slider.value;
    }
}
