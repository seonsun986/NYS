using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SH_SceneObj : MonoBehaviour
{
    // �� ������Ʈ �ȿ� �����ϴ� �͵� : InputField, Gameobject
    // �̶� InputField�� canvas�ȿ� �ٽ� �����Ѵ�'
    // ���ӿ�����Ʈ�� �� ������Ʈ �ڽ����� ������ InputField���� ��쿡�� Canvas���� �÷����ϴµ�
    int mySceneNum;
    public enum ObjType
    {
        text,
        obj,
    }

    public ObjType objType;

    // ��ư�� ��� ���� �ִϸ��̼��� �����Ѵ�
    // ���� : �ȱ�, �ٱ�, ����, �Ա�
    Animator anim;
    public GameObject animBtn;
    public string currentAnim;
    GameObject animBtnParent;

    // ���� ��ư
    public GameObject deleteBtn;
    // Instantiate�� ���� ��ư
    GameObject delete;

    // ���� ��ư�� �־� �� ����Ʈ
    public List<GameObject> buttons = new List<GameObject>();
    public List<AnimationClip> anims = new List<AnimationClip>();

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "EditorScene") return;

        if (GetComponent<SH_InputField>())
        {
            objType = ObjType.text;
        }
        else
        {
            objType = ObjType.obj;
        }
        // ���� ���� �ִ� Scene ��ȣ�� �����Ѵ�
        if (SH_BtnManager.Instance != null)
        {
            mySceneNum = int.Parse(gameObject.transform.parent.name.Substring(gameObject.transform.parent.name.Length - 1, 1));
        }


        // Ÿ���� ������Ʈ��� �ִϸ��̼� �����´�
        // �ִϸ��̼� ������ŭ ��ư�� �����Ѵ�
        // �ִϸ��̼��� ������ Ŭ���� �̸��� �������� ��ư�� �ѱ� �̸����� �ٲ��ش�
        if (objType.ToString() == "obj")
        {
            if (transform.childCount == 0 && GetComponent<Animator>() == null) return;
            if (transform.GetChild(0).GetComponent<Animator>() == null)
            {
                anim = GetComponent<Animator>();
            }

            else
            {
                anim = transform.GetChild(0).GetComponent<Animator>();
            }
            // ������ �� �⺻ �ִϸ��̼��� �ϳ� ��������ش�
            if(anim != null)
            anim.Play(gameObject.name.Substring(0, gameObject.name.Length - 7) + "_" +anims[0].name);
            for (int i = 0; i < anims.Count; i++)
            {
                // �� ������Ʈ�� ���ٸ� ������ְ�
                if (GameObject.Find(gameObject.name + "AnimBtn") == null)
                {
                    animBtnParent = new GameObject(gameObject.name + "AnimBtn");
                    // ������ų� �̹� �ִٸ� �θ� �������ش�
                    animBtnParent.transform.SetParent(GameObject.Find("AnimalAnimBtn").transform);
                    animBtnParent.transform.localPosition = new Vector3(0, 0, 0);
                }
                else
                {
                    animBtnParent = GameObject.Find(gameObject.name + "AnimBtn");
                }


                // �� ������Ʈ�� ���� ��ư�� �θ�� �������ش�
                GameObject animButton = Instantiate(animBtn);
                animButton.transform.SetParent(animBtnParent.transform);
                animButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-300 + (200 * i), 250, 0);

                // ex) WalkBtn ������ ����
                if (SH_EditorManager.Instance.animName.TryGetValue(anims[i].name, out string value))
                {
                    animButton.transform.GetChild(0).GetComponent<Text>().text = value;
                    animButton.name = anims[i].name + "Btn";
                }

                animButton.GetComponent<Button>().onClick.AddListener(PlayAnim);
                // ���� ��ư�� ��ư ����Ʈ�� �߰��Ѵ�
                buttons.Add(animButton);
            }

            if (GameObject.Find("GameManager") != null) return;
            else
            {
                // �����ϱ� ��ư�� ������ش�
                delete = Instantiate(deleteBtn);
                if (animBtnParent == null)
                {
                    animBtnParent = new GameObject(gameObject.name + "AnimBtn");
                    animBtnParent.transform.SetParent(GameObject.Find("AnimalAnimBtn").transform);
                    animBtnParent.transform.localPosition = new Vector3(0, 0, 0);
                }
                delete.transform.SetParent(animBtnParent.transform);
                delete.transform.localPosition = new Vector3(0, 170, 0);
                delete.GetComponent<Button>().onClick.AddListener(DeletePopUp);
                delete.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(PopUpYes);
                delete.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(PopUpNo);
                // ������ ���ڽ��ϱ� �˾��� ���д�
                delete.transform.GetChild(0).gameObject.SetActive(false);
                buttons.Add(delete);
            }
          
        }
    }

    int active;
    void Update()
    {
        if (SH_EditorManager.Instance == null) return;
        //if (objType.ToString() == "obj")
        //{
        //    // �ٽ� �ڱⰡ ���õǸ� �ִϸ��̼� ��ư�� �ѹ��� ���ش�
        //    if (SH_EditorManager.Instance.activeObj == gameObject && active < 1)
        //    {
        //        // �����ֱ�
        //        if (buttons[0].gameObject.activeSelf == true) return;

        //        for (int i = 0; i < buttons.Count; i++)
        //        {
        //            buttons[i].gameObject.SetActive(true);
        //        }
        //        active++;
        //    }

        // ���� ���õ� ������Ʈ�� �ڱ� �ڽ��� �ƴѰ�� �ִϸ��̼� ��ư�� ���ش�
        if (objType.ToString() == "obj")
        {
            if (SH_EditorManager.Instance.activeObj != gameObject && buttons.Count > 0)
            {
                // �����ֱ�
                if (buttons[0].gameObject.activeSelf == false) return;

                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].gameObject.SetActive(false);
                    if (i == buttons.Count - 1)
                    {
                        // ���� �����Ͻðڽ��ϱ� �˾��� �̸� �������Ѵ�
                        buttons[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }

                active = 0;
            }
        }
    }

    // ��ư�� Ŭ���Ѵ�� �ִϸ��̼��� ����ȴ�
    public void PlayAnim()
    {
        // ���õ� ��ư�� �̸��� ã�´�
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        // ��ư�� ���� �̸��� ã�ƿͼ� �� �ִϸ��̼��� �����Ų��(�̸� Dog_Walk)
        string clickName = clickBtn.name.Substring(0, clickBtn.name.Length - 3);
        string goName = gameObject.name.Substring(0, gameObject.name.Length - 7);
        anim.Play(goName + "_" + clickName);
        currentAnim = goName + "_" + clickName;
    }

    // ������ Delete��ư�� ������ ���� �����Ͻðڽ��ϱ� �˾��� �߰��Ѵ�
    public void DeletePopUp()
    {
        GameObject popUp = delete.transform.GetChild(0).gameObject;
        if (popUp.activeSelf == true) popUp.SetActive(false);
        else popUp.SetActive(true);
    }

    // Delete �˾� �ȿ��� �����ϱ� ���� ������ ���ӿ�����Ʈ �ı��ϰ� 
    // �ƴϿ� ������ �˾� ��ü�� ����
    public void PopUpYes()
    {
        // ���� ��ư�鵵 ��� �����ؾ��Ѵ�
        for(int i =0;i<buttons.Count;i++)
        {
            Destroy(buttons[i]);
        }
        Destroy(animBtnParent);
        Destroy(gameObject);
    }
    public void PopUpNo()
    {
        GameObject popUp = delete.transform.GetChild(0).gameObject;
        popUp.SetActive(false);
    }
}
