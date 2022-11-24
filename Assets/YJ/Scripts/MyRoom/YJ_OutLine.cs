using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_OutLine : MonoBehaviour
{
    Sprite originImage;
    public Sprite changeImage;
    public bool click = false;

    void Start()
    {
        originImage = GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickImageChange()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 0.9, "y", 0.9, "z", 0.9, "easeType", "easeOutSine", "time", 0.2f, "oncomplete", "SmallButton"));
        if (gameObject.GetComponent<Image>().sprite == originImage)
        {
            //SmallButton();
            gameObject.GetComponent<Image>().sprite = changeImage;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(365, 365);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = originImage;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        }
    }

    public void OnClickMTChange()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 0.9, "y", 0.9, "z", 0.9, "easeType", "easeOutSine", "time", 0.2f, "oncomplete", "SmallButton"));
        if (gameObject.GetComponent<Image>().sprite == originImage)
        {
            click = true;
            gameObject.GetComponent<Image>().sprite = changeImage;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        }
        else
        {
            click = false;
            gameObject.GetComponent<Image>().sprite = originImage;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
        }
    }

    void SmallButton()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "easeType", "easeOutSine", "time", 0.2f));
    }
}
