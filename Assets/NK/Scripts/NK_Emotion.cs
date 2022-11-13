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
            // 마우스 클릭하면
            if (Input.GetMouseButtonDown(0))
            {
                ClickUser();
            }

            if (emotion != NK_EmotionUI.emotion)
            {
                InitializationEmotion();
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

            // 클릭한 오브젝트의 LayerMask가 Player이고 자기자신이 아니면
            if (hit.transform.gameObject.layer == 6 && hit.transform.gameObject != gameObject)
            {
                clickUser = hit.transform.gameObject;
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
            if (emotion != Emotion.NoSelection && clickUser != null)
            {
                photonView.RPC("RPCShowEmotion", RpcTarget.All, emotion, photonView.ViewID, clickUser.GetPhotonView().ViewID);
            }
            yield return null;
        }
        NK_EmotionUI.emotion = Emotion.NoSelection;
        InitializationEmotion();
    }

    [PunRPC]
    private void RPCShowEmotion(Emotion currentEmotion, int myViewId, int clickUserViewId)
    {
        GameObject user = PhotonView.Find(clickUserViewId).gameObject;
        // 상대방을 마주봄
        user.transform.LookAt(transform);
        emojis[(int)currentEmotion - 1].SetActive(true);
        GameObject subject = PhotonView.Find(myViewId).gameObject;
        GameObject avt = subject.GetComponent<YJ_PlayerAvatarSet>().avt;
        // 표정 변화
        avt.transform.GetChild(1).transform.GetChild(1).GetComponent<Renderer>().material = expressions[(int)currentEmotion - 1];
        // 애니메이션 변화
        avt.GetComponent<Animator>().SetBool(currentEmotion.ToString(), true);
    }

    private void InitializationEmotion()
    {
        if (emotion != Emotion.NoSelection)
        {
            photonView.RPC("RPCInitializationEmotion", RpcTarget.All, emotion, photonView.ViewID);
            emotion = Emotion.NoSelection;
        }
    }

    [PunRPC]
    private void RPCInitializationEmotion(Emotion currentEmotion, int myViewId)
    {
        emojis[(int)currentEmotion - 1].SetActive(false);
        GameObject subject = PhotonView.Find(myViewId).gameObject;
        GameObject avt = subject.GetComponent<YJ_PlayerAvatarSet>().avt;
        // 애니메이션 초기화
        avt.GetComponent<Animator>().SetBool(currentEmotion.ToString(), false);
        // 표정 초기화
        avt.transform.GetChild(1).transform.GetChild(1).GetComponent<Renderer>().material = expressions[0];
    }
}
