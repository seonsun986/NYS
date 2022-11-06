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
    public List<GameObject> emojis = new List<GameObject>();
    public Animator anim;
    public float emotionTime = 2f;
    bool mouseClick;
    float currentTime;
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
            //clickUser.transform.forward = - gameObject.transform.forward;
            emotion = NK_EmotionUI.emotion;
            ShowEmotion();
            currentTime += Time.deltaTime;
        }

        if(emotionTime < currentTime)
        {
            InitializationEmotion();
            currentTime = 0;
        }
    }

    private void ClickUser()
    {
        emotionUI.gameObject.SetActive(true);
        emotionUI.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        mouseClick = false;
        /*        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
                }*/
    }

    private void ShowEmotion()
    {
        emojis[(int)emotion - 1].SetActive(true);
        anim.SetTrigger(emotion.ToString());
    }

    private void InitializationEmotion()
    {
        emojis[(int)emotion - 1].SetActive(false);
        anim.SetTrigger("Idle");
        emotion = Emotion.NoSelection;
    }
}
