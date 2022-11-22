using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Stage : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        iTween.ScaleFrom(gameObject, iTween.Hash("x", 0, "easeType", "easeInOutBack"));
    }
}
