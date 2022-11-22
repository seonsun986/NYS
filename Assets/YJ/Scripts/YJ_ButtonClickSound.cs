using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_ButtonClickSound : MonoBehaviour
{
    // ȿ���� ��� ���� ��ũ��Ʈ
    AudioSource audioSet;

    [SerializeField]
    [Header("BGM")]
    public AudioClip startButtonSound;
    public AudioClip buttonSound;


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

}
