using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_EditorManager : MonoBehaviour
{
    public static SH_EditorManager Instance;

    #region 텍스트 관련 선언 변수
    public SH_InputField active_InputField;     // 현재 선택된 InputField
    public Dropdown font;                       // 폰트
    public Text fontSize;                       // 폰트 사이즈
    public Font[] fonts;
    #endregion

    // 생성된 오브젝트 애니메이션 담아놓는다
    [Header("오브젝트 애니메이션")]
    public List<Animation> anim = new List<Animation>();
    // 딕셔너리 선언해서 Walk -> 걷기
    public Dictionary<string, string> animName;

    // 현재 클릭되어있는 오브젝트
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
            { "Walk", "걷기"},
            { "Run", "뛰기"},
            {"Eat", "먹기" },
            { "Jump", "점프" },
            {"Idle", "정지" },
            { "Idle2" ,"정지2"},
            {"No", "절레절레" },
            {"Sit", "앉기" },
            { "Attack", "공격"},
            {"Water", "물 마시기" },
            {"Slash", "던지기" },
            {"Fly", "날기" },
            {"Gritar", "포효" },
            { "Shout", "포효"},
            { "Clap","박수치기"},
            { "Pray","기도하기"},
            { "Dance","춤추기"},
            { "Hi","인사하기"},
            { "Yawn", "기지개펴기"},
            { "Kiss", "뽀뽀날리기"},
            { "Cry", "울기"},
            { "Fight", "싸우기"},
            { "SneakWalk", "살금살금 걷기"},
        };
    }

    void Update()
    {
        if (active_InputField != null)
        {
            // 현재 선택되어 있는 InputField의 값을 바꿔보자
            active_InputField.info.txtDropdown = font.value;
            active_InputField.transform.GetChild(3).GetComponent<Text>().font = fonts[active_InputField.info.txtDropdown];
            active_InputField.info.txtSize = int.Parse(fontSize.text);
            active_InputField.transform.GetChild(3).GetComponent<Text>().fontSize = active_InputField.info.txtSize;
        }

        // 클릭되어있는 오브젝트 구하기
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);          
            if(Physics.Raycast(ray, out hitInfo) && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Object"))
            {
                activeObj = hitInfo.transform.gameObject;
                activeObj_anim = activeObj.GetComponent<SH_SceneObj>().currentAnim;
            }
                
        }
    }
    
}
