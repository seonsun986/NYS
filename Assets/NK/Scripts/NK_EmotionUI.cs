using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EmotionUI : MonoBehaviour
{
    public static NK_Emotion.Emotion emotion = NK_Emotion.Emotion.NoSelection;
    public float emotionTime = 0.5f;
    float currentTime = 0f;
    int i = 0;

    void Update()
    {
        if (gameObject.activeSelf && i < 5)
        {
            currentTime += Time.deltaTime;

            if (currentTime > emotionTime)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                currentTime = 0f;
                i++;
            }
        }
    }

    public void ClickHi()
    {
        emotion = NK_Emotion.Emotion.Hi;
        gameObject.SetActive(false);
    }
    public void ClickHappy()
    {
        emotion = NK_Emotion.Emotion.Happy;
        gameObject.SetActive(false);
    }
    public void ClickSad()
    {
        emotion = NK_Emotion.Emotion.Sad;
        gameObject.SetActive(false);
    }
    public void ClickDefeat()
    {
        emotion = NK_Emotion.Emotion.Defeat;
        gameObject.SetActive(false);
    }
    public void ClickExcite()
    {
        emotion = NK_Emotion.Emotion.Excite;
        gameObject.SetActive(false);
    }
}
