using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_EditorManager : MonoBehaviour
{
    public static SH_EditorManager Instance;

    #region �ؽ�Ʈ ���� ���� ����
    public SH_InputField active_InputField;     // ���� ���õ� InputField
    public Dropdown font;                       // ��Ʈ
    public Text fontSize;                       // ��Ʈ ������
    public Font[] fonts;
    #endregion

    // ������ ������Ʈ �ִϸ��̼� ��Ƴ��´�
    [Header("������Ʈ �ִϸ��̼�")]
    public List<Animation> anim = new List<Animation>();
    // ��ųʸ� �����ؼ� Walk -> �ȱ�
    public Dictionary<string, string> animName;
    
    // ������ ��ư�� ���� Instantiate�Ǵ� ģ������ �޶�����
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
        };
    }

    void Update()
    {
        if (active_InputField != null)
        {
            // ���� ���õǾ� �ִ� InputField�� ���� �ٲ㺸��
            active_InputField.info.txtDropdown = font.value;
            active_InputField.transform.GetChild(3).GetComponent<Text>().font = fonts[active_InputField.info.txtDropdown];
            active_InputField.info.txtSize = int.Parse(fontSize.text);
            active_InputField.transform.GetChild(3).GetComponent<Text>().fontSize = active_InputField.info.txtSize;
        }
    }
    
}
