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
    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        emotionUI = GameObject.Find("Canvas").transform.Find("EmotionUI");
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && emotionUI != null)
        {
            // 상호작용 UI 유저 머리 위에 띄우기
            emotionUI.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0, 1, 0));

            // 마우스 클릭하면
            if (Input.GetMouseButtonDown(0))
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
    }

    private void ClickUser()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            clickUser = hit.transform.gameObject;

            // LayerMask가 Player이면
            if (hit.transform.gameObject.layer == 6)
            {
                emotionUI.gameObject.SetActive(true);
            }
            else if(hit.transform.gameObject.layer == 5)
            {
                return;
            }
            else
            {
                emotionUI.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator ShowEmotion()
    {
        currentTime = 0;

        while (emotionTime > currentTime)
        {
            currentTime += Time.deltaTime;
            if (emotion != Emotion.NoSelection)
            {
                photonView.RPC("RPCShowEmotion", RpcTarget.All);
            }
            yield return null;
        }
        NK_EmotionUI.emotion = Emotion.NoSelection;
        InitializationEmotion();
    }

    [PunRPC]
    private void RPCShowEmotion()
    {
        emojis[(int)emotion - 1].SetActive(true);
        face.GetComponent<Renderer>().material = expressions[(int)emotion - 1];
        anim.SetBool(emotion.ToString(), true);
    }

    private void InitializationEmotion()
    {
        if (emotion != Emotion.NoSelection)
        {
            photonView.RPC("RPCInitializationEmotion", RpcTarget.All);
            emotion = Emotion.NoSelection;
        }
    }

    [PunRPC]
    private void RPCInitializationEmotion()
    {
        emojis[(int)emotion - 1].SetActive(false);
        anim.SetBool(emotion.ToString(), false);
        face.GetComponent<Renderer>().material = expressions[0];
    }
}
