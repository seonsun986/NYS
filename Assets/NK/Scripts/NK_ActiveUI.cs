using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_ActiveUI : MonoBehaviour
{
    private void OnEnable()
    {
        iTween.ScaleFrom(gameObject, iTween.Hash("x", 0.1f, "y", 0.1f, "z", 0.1f, "time", 0.5f));
    }
}
