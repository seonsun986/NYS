using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EmotionUI : MonoBehaviour
{
    public static NK_Emotion.Emotion emotion = NK_Emotion.Emotion.NoSelection;
    public void ClickHi()
    {
        emotion = NK_Emotion.Emotion.Hi;
    }
    public void ClickHappy()
    {
        emotion = NK_Emotion.Emotion.Happy;
    }
    public void ClickSad()
    {
        emotion = NK_Emotion.Emotion.Sad;
    }
    public void ClickDefeat()
    {
        emotion = NK_Emotion.Emotion.Defeat;
    }
    public void ClickExcite()
    {
        emotion = NK_Emotion.Emotion.Excite;
    }
}
