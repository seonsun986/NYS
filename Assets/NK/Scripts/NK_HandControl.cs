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

        for (int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            if (GameManager.Instance.children[i].Owner.NickName == transform.GetComponentInChildren<Text>().text)
            {
                if (GameManager.Instance.children[i].GetComponent<NK_PlayerMove>().state == NK_PlayerMove.State.HandUp)
                {
                    handImage.sprite = handupSprite;
                }
            }
        }
    }

    private void Update()
    {
    }

    public void ChildHandDown()
    {
        handImage.sprite = handdownSprite;
        NK_UIController.instance.ClickHandDown(transform.GetComponentInChildren<Text>().text);
    }
}
