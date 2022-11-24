using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NK_SoundOnOff : MonoBehaviour
{
    #region BGM On/Off
    public Toggle bgmOnOff;
    public GameObject bgmOn, bgmOff;
    // 228, 291
    // πË∞Ê¿Ωæ« On/Off
    public void BGMOnAndOff()
    {
        if (bgmOnOff.isOn)
        {
            YJ_AudioManager.instance.bgmOnOff = true;
            //Camera.main.GetComponent<AudioSource>().volume = 1;
            bgmOff.SetActive(false);
            bgmOn.SetActive(true);
        }
        else
        {
            YJ_AudioManager.instance.bgmOnOff = false;
            //Camera.main.GetComponent<AudioSource>().volume = 0;
            bgmOn.SetActive(false);
            bgmOff.SetActive(true);
        }
    }
    #endregion

    #region EffectSound On/Off
    public Toggle effectOnOff;
    public GameObject effectOn, effectOff;

    GameObject canvas;

    // 228, 291
    // πË∞Ê¿Ωæ« On/Off
    public void EffectOnAndOff()
    {
        canvas = GameObject.Find("Canvas");
        if (effectOnOff.isOn)
        {
            YJ_AudioManager.instance.effectOnOff = true;
            //canvas.GetComponent<AudioSource>().volume = 1;
            effectOff.SetActive(false);
            effectOn.SetActive(true);
        }
        else
        {
            YJ_AudioManager.instance.effectOnOff = false;
            //canvas.GetComponent<AudioSource>().volume = 0;
            effectOn.SetActive(false);
            effectOff.SetActive(true);
        }
    }
    #endregion
}
