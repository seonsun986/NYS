using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EmotionSound : MonoBehaviourPun
{
    // Start is called before the first frame update
    void OnEnable()
    {
        if (!YJ_AudioManager.instance.effectOnOff)
        {
            gameObject.GetComponent<AudioSource>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
        }
    }
}
