using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SH_EditorManager : MonoBehaviour
{
    public static SH_EditorManager Instance;

    #region 텍스트 관련 선언 변수
    public SH_InputField active_InputField;     // 현재 선택된 InputField(바뀐)
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
        if(Instance == null)
        {
            Instance = this;
        }
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
            {"DoongDoong", "떠다니기" },
            { "Headtilt", "갸우뚱"},
            { "Lay", "눕기"},
            {"Yell", "소리지르기" },
        };
    }
    int i = 0;
    public GameObject gizmo1;
    public GameObject gizmo2;
    public GameObject gizmo3;
    public GameObject gizmo4;
    
    void Update()
    {
     

        // 클릭되어있는 오브젝트 구하기
        // 1.
        if (Input.GetMouseButtonDown(0))
        {
            // 1. 일단 눌렀을 때 오브젝트라면 현재 오브젝트를 바꾸자
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
                    print("클릭한 오브젝트 : " + hitInfo.transform.gameObject.name);
                    activeObj = hitInfo.transform.gameObject;
                    // 항상 선택된 오브젝트의 버튼은 켜주자
                    for (int i = 0; i < activeObj.GetComponent<SH_SceneObj>().buttons.Count; i++)
                    {
                        activeObj.GetComponent<SH_SceneObj>().buttons[i].SetActive(true);
                    }
                }
                
                // 오브젝트가 아닐때
                else
                {
                    if (activeObj == null) return;
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
                                if(j == activeObj.GetComponent<SH_SceneObj>().buttons.Count-1)
                                {
                                    // 정말 삭제하시겠습니까 팝업도 미리 꺼놔야한다
                                    activeObj.GetComponent<SH_SceneObj>().buttons[j].transform.GetChild(0).gameObject.SetActive(false);
                                }
                                
                            }
                        }
                    }
                }
                
            }
            
        }
    }
}
