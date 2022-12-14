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
        else if(transform.root.gameObject.GetPhotonView().IsMine)
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
        }
    }
}
