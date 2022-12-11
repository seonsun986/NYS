using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_BtnEffect : MonoBehaviour
{
    public void OnClickMTChange(float bigSize/*, float smallSize*/)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", bigSize, "y", bigSize, "z", bigSize, "easeType", "easeOutSine", "time", 0.2f));
        StartCoroutine(SmallButton(bigSize - 0.1f));
    }

    IEnumerator SmallButton(float smallsize)
    {
        yield return new WaitForSeconds(0.2f);
        iTween.ScaleTo(gameObject, iTween.Hash("x", smallsize, "y", smallsize, "z", smallsize, "easeType", "easeOutSine", "time", 0.2f));
    }
}
