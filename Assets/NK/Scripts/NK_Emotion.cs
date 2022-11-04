using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Emotion : MonoBehaviourPun
{
    public enum Emotion
    {
        NoSelection,
        Hi,
        Happy,
        Sad,
        Defeat,
        Excite,
    }

    public Transform emotionUI;
    public GameObject clickUser;
    public Emotion emotion = Emotion.NoSelection;
    bool mouseClick;
    // Start is called before the first frame update
    void Start()
    {
        emotionUI = GameObject.Find("Canvas").transform.Find("EmotionUI");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseClick = true;
        }

        if(mouseClick && emotionUI != null)
        {
            ClickUser();
        }

        if(NK_EmotionUI.emotion != Emotion.NoSelection)
        {
            clickUser.transform.forward = - gameObject.transform.forward;
        }
    }

    private void ClickUser()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            clickUser = hit.transform.gameObject;

            if (hit.transform.gameObject.tag == "Child")
            {
                emotionUI.gameObject.SetActive(true);
                emotionUI.transform.position = gameObject.transform.position;
                mouseClick = false;
            }
        }
    }
}
