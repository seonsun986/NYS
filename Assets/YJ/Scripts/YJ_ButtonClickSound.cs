using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_ButtonClickSound : MonoBehaviour
{
    // 효과음 재생 전용 스크립트
    AudioSource audioSet;

    [SerializeField]
    [Header("BGM")]
    public AudioClip startButtonSound;
    public AudioClip buttonSound;
    public AudioClip oBtnSound;
    public AudioClip xBtnSound;

    void Start()
    {
        audioSet = GetComponent<AudioSource>();
    }

    public void OnClickStartSound()
    {
        audioSet.clip = startButtonSound;
        audioSet.Play();
    }

    public void OnClickSound()
    {
        if (audioSet == null)
        {
            audioSet = GetComponent<AudioSource>();
        }
        audioSet.clip = buttonSound;
        audioSet.Play();
    }

    public void OBtnClickSound()
    {
        audioSet.clip = oBtnSound;
        audioSet.Play();
    }

    public void XBtnClickSound()
    {
        audioSet.clip = xBtnSound;
        audioSet.Play();
    }

}
