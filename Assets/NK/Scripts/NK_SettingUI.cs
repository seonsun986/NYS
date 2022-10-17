using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NK_SettingUI : MonoBehaviour
{
    public Slider slider;
    // 자신만의 Speaker로 변환
    public GameObject Speaker;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = Speaker.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickMinus()
    {
        slider.value -= 0.1f;
        audioSource.volume = slider.value;
    }

    public void ClickPlus()
    {
        slider.value += 0.1f;
        audioSource.volume = slider.value;
    }
}
