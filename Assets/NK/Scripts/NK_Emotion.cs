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
    public List<Material> expressions = new List<Material>();
    public Animator anim;
    public GameObject face;
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
        if (Input.GetMouseButtonDown(0))
        {
            mouseClick = true;
        }

        if (mouseClick && emotionUI != null)
        {
            ClickUser();
        }

        if (emotion != NK_EmotionUI.emotion)
        {
            InitializationEmotion();
            //clickUser.transform.forward = - gameObject.transform.forward;
            emotion = NK_EmotionUI.emotion;
            StopAllCoroutines();
            StartCoroutine(ShowEmotion());
        }
    }

    private void ClickUser()
    {
        emotionUI.gameObject.SetActive(true);
        emotionUI.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0,1,0));
        mouseClick = false;
        /*        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    clickUser = hit.transform.gameObject;

                    if (hit.transform.gameObject.tag == "Child")
                    {
                        emotionUI.gameObject.SetActive(true);
                        emotionUI.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0,1,0));
                        mouseClick = false;
                    }
                }*/
    }

    private IEnumerator ShowEmotion()
    {
        currentTime = 0;

        while (emotionTime > currentTime)
        {
            currentTime += Time.deltaTime;
            if (emotion != Emotion.NoSelection)
            {
                emojis[(int)emotion - 1].SetActive(true);
                face.GetComponent<Renderer>().material = expressions[(int)emotion - 1];
                anim.SetBool(emotion.ToString(), true);
            }
            yield return null;
        }
        NK_EmotionUI.emotion = Emotion.NoSelection;
        InitializationEmotion();
    }

    private void InitializationEmotion()
    {
        if (emotion != Emotion.NoSelection)
        {
            emojis[(int)emotion - 1].SetActive(false);
            anim.SetBool(emotion.ToString(), false);
            face.GetComponent<Renderer>().material = expressions[0];
            emotion = Emotion.NoSelection;
            print("dd");
        }
    }
}
