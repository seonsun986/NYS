using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
                // 상대방을 마주봄
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

        if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            clickUser = hit.transform.gameObject;

            // LayerMask가 Player이면
            if (hit.transform.gameObject.layer == 6 && hit.transform.gameObject != gameObject)
            {
                emotionUI.gameObject.SetActive(true);
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
                photonView.RPC("RPCShowEmotion", RpcTarget.All, emotion, clickUser.GetPhotonView().ViewID);
            }
            yield return null;
        }
        NK_EmotionUI.emotion = Emotion.NoSelection;
        InitializationEmotion();
    }

    [PunRPC]
    private void RPCShowEmotion(Emotion currentEmotion, int viewId)
    {
        GameObject user = PhotonView.Find(viewId).gameObject;
        user.transform.LookAt(transform);
        emojis[(int)currentEmotion - 1].SetActive(true);
        face.GetComponent<Renderer>().material = expressions[(int)currentEmotion - 1];
        anim.SetBool(currentEmotion.ToString(), true);
    }

    private void InitializationEmotion()
    {
        if (emotion != Emotion.NoSelection)
        {
            photonView.RPC("RPCInitializationEmotion", RpcTarget.All, emotion);
            emotion = Emotion.NoSelection;
        }
    }

    [PunRPC]
    private void RPCInitializationEmotion(Emotion currentEmotion)
    {
        emojis[(int)currentEmotion - 1].SetActive(false);
        anim.SetBool(currentEmotion.ToString(), false);
        face.GetComponent<Renderer>().material = expressions[0];
    }
}
