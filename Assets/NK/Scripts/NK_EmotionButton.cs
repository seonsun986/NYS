using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EmotionButton : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        // 크기 커지면서 보이도록
        iTween.ScaleFrom(gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", "easeInOutBack"));
    }

    // Update is called once per frame
    void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
