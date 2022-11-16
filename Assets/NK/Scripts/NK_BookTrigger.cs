using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BookTrigger : MonoBehaviour
{
    public void PointEnterBook()
    {
        // 크기 커지면서 보이도록
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1.2, "y", 1.2, "z", 1.2, "easeType", "easeOutExpo", "time", 0.5f));
    }

    public void PointExitBook()
    {
        // 크기 다시 작아짐
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "easeType", "easeOutExpo", "time", 0.5f));
    }
}
