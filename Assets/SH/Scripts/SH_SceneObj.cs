using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    // ���� ��ư�� �־� �� ����Ʈ
    public List<GameObject> buttons = new List<GameObject>();
    public List<AnimationClip> anims = new List<AnimationClip>();
    
    void Start()
    {

        if (GetComponent<SH_InputField>())
        {
            objType = ObjType.text;
        }
        else
        {
            objType = ObjType.obj;
        }
        // ���� ���� �ִ� Scene ��ȣ�� �����Ѵ�
        if(SH_BtnManager.Instance != null)
        {
            mySceneNum = int.Parse(gameObject.transform.parent.name.Substring(gameObject.transform.parent.name.Length - 1, 1));
        }

        // Ÿ���� ������Ʈ��� �ִϸ��̼� �����´�
        // �ִϸ��̼� ������ŭ ��ư�� �����Ѵ�
        // �ִϸ��̼��� ������ Ŭ���� �̸��� �������� ��ư�� �ѱ� �̸����� �ٲ��ش�
        if(objType.ToString() == "obj")
        {
            anim = GetComponent<Animator>();
            for(int i =0;i<anims.Count;i++)
            {
                // �� ������Ʈ�� ���ٸ� ������ְ�
                if(GameObject.Find(gameObject.name + "AnimBtn") == null)
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
                animButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-300 + (200 * i), -400,0);

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
        }
    }

    void Update()
    {
        if (objType.ToString() == "obj")
        {
            // �ٽ� �ڱⰡ ���õǸ� �ִϸ��̼� ��ư�� ���ش�
            if (SH_EditorManager.Instance.activeObj == gameObject)
            {
                // �����ֱ�
                if (buttons[0].gameObject.activeSelf == true) return;

                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].gameObject.SetActive(true);
                }
            }
            // ���� ���õ� ������Ʈ�� �ڱ� �ڽ��� �ƴѰ�� �ִϸ��̼� ��ư�� ���ش�
            else
            {
                // �����ֱ�
                if (buttons[0].gameObject.activeSelf == false) return;

                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].gameObject.SetActive(false);
                }
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
        anim.Play(goName + "_"+ clickName);
        currentAnim = goName + "_" + clickName;
    }
}
