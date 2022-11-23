using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_ActiveUI : MonoBehaviour
{
    private void OnEnable()
    {
        iTween.ScaleFrom(gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));
    }
}
