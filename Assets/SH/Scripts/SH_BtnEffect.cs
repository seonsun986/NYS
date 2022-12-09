using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_BtnEffect : MonoBehaviour
{
    public void OnClickMTChange()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1.55f, "y", 1.55f, "z", 1.55f, "easeType", "easeOutSine", "time", 0.2f, "oncomplete", "SmallButton"));

    }

    void SmallButton()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1.4154f, "y", 1.4154f, "z", 1.4154f, "easeType", "easeOutSine", "time", 0.2f));
    }
}
