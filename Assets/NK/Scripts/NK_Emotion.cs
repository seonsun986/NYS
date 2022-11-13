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
            // ���콺 Ŭ���ϸ�
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

            // Ŭ���� ������Ʈ�� LayerMask�� Player�̰� �ڱ��ڽ��� �ƴϸ�
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
        // ������ ���ֺ�
        user.transform.LookAt(transform);
        emojis[(int)currentEmotion - 1].SetActive(true);
        GameObject subject = PhotonView.Find(myViewId).gameObject;
        GameObject avt = subject.GetComponent<YJ_PlayerAvatarSet>().avt;
        // ǥ�� ��ȭ
        avt.transform.GetChild(1).transform.GetChild(1).GetComponent<Renderer>().material = expressions[(int)currentEmotion - 1];
        // �ִϸ��̼� ��ȭ
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
        // �ִϸ��̼� �ʱ�ȭ
        avt.GetComponent<Animator>().SetBool(currentEmotion.ToString(), false);
        // ǥ�� �ʱ�ȭ
        avt.transform.GetChild(1).transform.GetChild(1).GetComponent<Renderer>().material = expressions[0];
    }
}
