using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NK_HandControl : MonoBehaviour
{
    public Image handImage;
    public Sprite handupSprite;
    public Sprite handdownSprite;

    private void Start()
    {
        handImage.sprite = handdownSprite;
    }

    private void Update()
    {
    }

    public void ChildHandDown()
    {
        NK_UIController.instance.ClickHandDown(transform.GetComponentInChildren<Text>().text);
    }
}
