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
    Animation anim;
    public GameObject animBtn;
    GameObject animBtnParent;
    // ���� ��ư�� �־� �� ����Ʈ
    public List<GameObject> buttons = new List<GameObject>();
    void Start()
    {
        if(GetComponent<SH_InputField>())
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
            anim = GetComponent<Animation>();
            for(int i =0;i<anim.GetClipCount();i++)
            {
                // �� ������Ʈ�� ���ٸ� ������ְ�
                if(GameObject.Find(gameObject.name + "AnimBtn") == null)
                {                    
                    animBtnParent = new GameObject(gameObject.name + "AnimBtn");
                }
                // ������ų� �̹� �ִٸ� �θ� �������ش�
                animBtnParent.transform.SetParent(GameObject.Find("AnimalAnimBtn").transform);
                animBtnParent.transform.localPosition = new Vector3(0, 0, 0);

                // �� ������Ʈ�� ���� ��ư�� �θ�� �������ش�
                GameObject animButton = Instantiate(animBtn);
                animButton.transform.SetParent(animBtnParent.transform);
                animButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-300 + (200 * i), -400,0);
                int j = 0;
                foreach (AnimationState state in anim)
                {
                    if(j==i)
                    {
                        animButton.transform.GetChild(0).GetComponent<Text>().text = SH_EditorManager.Instance.animName[state.name];
                        animButton.name = state.name + "Btn";
                    }
                    j++;
                }

                animButton.GetComponent<Button>().onClick.AddListener(PlayAnim);
                // ���� ��ư�� ��ư ����Ʈ�� �߰��Ѵ�
                buttons.Add(animButton);
            }
        }
    }

    void Update()
    {
        
    }

    // ��ư�� Ŭ���Ѵ�� �ִϸ��̼��� ����ȴ�
    public void PlayAnim()
    {
        // ���õ� ��ư�� �̸��� ã�´�
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        // ��ư�� ���� �̸��� ã�ƿͼ� �� �ִϸ��̼��� �����Ų��
        string clickName = clickBtn.name.Substring(0, clickBtn.name.Length - 3);
        anim.Play(clickName);
    }
}
