using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SH_EditorManager : MonoBehaviour
{
    public static SH_EditorManager Instance;

    #region �ؽ�Ʈ ���� ���� ����
    public SH_InputField origin_InputField;
    public SH_InputField active_InputField;     // ���� ���õ� InputField(�ٲ�)
    public Dropdown font;                       // ��Ʈ
    public string fontSize;                       // ��Ʈ ������
    public Color fontColor;
    public InputField InputfontSize;
    public Image fontColorImage;
    public Font[] fonts;
    #endregion

    // ������ ������Ʈ �ִϸ��̼� ��Ƴ��´�
    [Header("������Ʈ �ִϸ��̼�")]
    public List<Animation> anim = new List<Animation>();
    // ��ųʸ� �����ؼ� Walk -> �ȱ�
    public Dictionary<string, string> animName;

    // ���� Ŭ���Ǿ��ִ� ������Ʈ
    public GameObject activeObj;
    public string activeObj_anim;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        animName = new Dictionary<string, string>
        {
            { "Walk", "�ȱ�"},
            { "Run", "�ٱ�"},
            {"Eat", "�Ա�" },
            { "Jump", "����" },
            {"Idle", "����" },
            { "Idle2" ,"����2"},
            {"No", "��������" },
            {"Sit", "�ɱ�" },
            { "Attack", "����"},
            {"Water", "�� ���ñ�" },
            {"Slash", "������" },
            {"Fly", "����" },
            {"Gritar", "��ȿ" },
            { "Shout", "��ȿ"},
            { "Clap","�ڼ�ġ��"},
            { "Pray","�⵵�ϱ�"},
            { "Dance","���߱�"},
            { "Hi","�λ��ϱ�"},
            { "Yawn", "���������"},
            { "Kiss", "�ǻǳ�����"},
            { "Cry", "���"},
            { "Fight", "�ο��"},
            { "SneakWalk", "��ݻ�� �ȱ�"},
        };
    }
    int i = 0;
    public GameObject gizmo1;
    public GameObject gizmo2;
    public GameObject gizmo3;
    public GameObject gizmo4;
    
    void Update()
    {
        if (active_InputField != null)
        {
            if (origin_InputField.name != active_InputField.name && origin_InputField != null)
            {
                origin_InputField = active_InputField;
                font.value = origin_InputField.info.txtDropdown;
                fontSize = active_InputField.info.txtSize.ToString();
                fontColorImage.color = active_InputField.info.txtColor;
            }
            // ���� ���õǾ� �ִ� InputField�� ���� �ٲ㺸��
            // ó�� ��������ٸ� �⺻ ������ �����Ű�� 
            // ���� ��������־��ٸ� �ڱⰡ ������ �ִ� ������ �ٽ� �ҷ��´�
            if (active_InputField.transform.childCount == 3) return;

            active_InputField.info.txtDropdown = font.value;
            active_InputField.transform.GetChild(3).GetComponent<Text>().font = fonts[active_InputField.info.txtDropdown];
            active_InputField.info.txtSize = int.Parse(fontSize);
            active_InputField.transform.GetChild(3).GetComponent<Text>().fontSize = active_InputField.info.txtSize;
            active_InputField.info.txtColor = fontColorImage.color;
            active_InputField.transform.GetChild(3).GetComponent<Text>().color = active_InputField.info.txtColor;

        }

        // Ŭ���Ǿ��ִ� ������Ʈ ���ϱ�
        // 1.
        if (Input.GetMouseButtonDown(0))
        {
            // 1. �ϴ� ������ �� ������Ʈ��� ���� ������Ʈ�� �ٲ���
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            if (Physics.Raycast(ray, out hitInfo))
            {
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Object"))
                {
                    print("Ŭ���� ������Ʈ : " + hitInfo.transform.gameObject.name);
                    activeObj = hitInfo.transform.gameObject;
                    // �׻� ���õ� ������Ʈ�� ��ư�� ������
                    for (int i = 0; i < activeObj.GetComponent<SH_SceneObj>().buttons.Count; i++)
                    {
                        activeObj.GetComponent<SH_SceneObj>().buttons[i].SetActive(true);
                    }
                }
                
                // ������Ʈ�� �ƴҶ�
                else
                {
                    if (activeObj.gameObject == null) return;
                    for (int i = 0; i < raycastResults.Count; i++)
                    {
                        if (raycastResults[i].gameObject.layer == LayerMask.NameToLayer("Button"))
                        {
                            return;
                        }
                        else
                        {
                            for(int j =0;j<activeObj.GetComponent<SH_SceneObj>().buttons.Count;j++)
                            {
                                activeObj.GetComponent<SH_SceneObj>().buttons[j].SetActive(false);
                            }
                        }
                    }
                }
                
            }

           

            
        }
       
     
    }

 
    
}
