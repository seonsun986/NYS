using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_ClickBTN : MonoBehaviour
{
    Image image;
    public Sprite click_On;
    public Sprite click_Off;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = click_Off;
    }
    // ���Ұ� ���ο� ���� ��ư �̹��� ����
    public void ChangeImage()
    {
        if (image.sprite == click_Off)
        {
            image.sprite = click_On;
        }
        else
        {
            image.sprite = click_Off;
        }
    }
}
