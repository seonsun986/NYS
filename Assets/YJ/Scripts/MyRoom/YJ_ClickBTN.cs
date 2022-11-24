using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_ClickBTN : MonoBehaviour
{
    public Image image;
    public Sprite click_On;
    public Sprite click_Off;

    public Image otherBTN_1;
    public Image otherBTN_2;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = click_Off;
    }
    // 버튼 이미지 변경
    public void ChangeImage()
    {
        if (image.sprite == click_Off)
        {
            image.sprite = click_On;
            otherBTN_1.GetComponent<YJ_ClickBTN>().image.sprite = otherBTN_1.GetComponent<YJ_ClickBTN>().click_Off;
            otherBTN_2.GetComponent<YJ_ClickBTN>().image.sprite = otherBTN_2.GetComponent<YJ_ClickBTN>().click_Off;
        }
        else
        {
            image.sprite = click_Off;
        }
    }

}
