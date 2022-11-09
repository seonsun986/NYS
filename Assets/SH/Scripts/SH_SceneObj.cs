using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SH_SceneObj : MonoBehaviour
{
    // 빈 오브젝트 안에 들어가야하는 것들 : InputField, Gameobject
    // 이때 InputField는 canvas안에 다시 들어가야한다'
    // 게임오브젝트는 빈 오브젝트 자식으로 넣지만 InputField같은 경우에는 Canvas에서 올려야하는데
    int mySceneNum;
    public enum ObjType
    {
        text,
        obj,
    }

    public ObjType objType;

    // 버튼의 제어에 따라서 애니메이션을 결정한다
    // 동물 : 걷기, 뛰기, 점프, 먹기
    Animator anim;
    public GameObject animBtn;
    public string currentAnim;
    GameObject animBtnParent;

    // 삭제 버튼
    public GameObject deleteBtn;
    // Instantiate된 삭제 버튼
    GameObject delete;

    // 만든 버튼을 넣어 줄 리스트
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
        // 내가 속해 있는 Scene 번호를 저장한다
        if (SH_BtnManager.Instance != null)
        {
            mySceneNum = int.Parse(gameObject.transform.parent.name.Substring(gameObject.transform.parent.name.Length - 1, 1));
        }


        // 타입이 오브젝트라면 애니메이션 가져온다
        // 애니메이션 개수만큼 버튼을 생성한다
        // 애니메이션의 각각의 클립의 이름을 바탕으로 버튼을 한글 이름으로 바꿔준다
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
            // 시작할 때 기본 애니메이션을 하나 재생시켜준다
            if(anim != null)
            anim.Play(gameObject.name.Substring(0, gameObject.name.Length - 7) + "_" +anims[0].name);
            for (int i = 0; i < anims.Count; i++)
            {
                // 빈 오브젝트가 없다면 만들어주고
                if (GameObject.Find(gameObject.name + "AnimBtn") == null)
                {
                    animBtnParent = new GameObject(gameObject.name + "AnimBtn");
                    // 만들었거나 이미 있다면 부모를 설정해준다
                    animBtnParent.transform.SetParent(GameObject.Find("AnimalAnimBtn").transform);
                    animBtnParent.transform.localPosition = new Vector3(0, 0, 0);
                }
                else
                {
                    animBtnParent = GameObject.Find(gameObject.name + "AnimBtn");
                }


                // 빈 오브젝트를 만들 버튼의 부모로 설정해준다
                GameObject animButton = Instantiate(animBtn);
                animButton.transform.SetParent(animBtnParent.transform);
                animButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-300 + (200 * i), 250, 0);

                // ex) WalkBtn 등으로 나옴
                if (SH_EditorManager.Instance.animName.TryGetValue(anims[i].name, out string value))
                {
                    animButton.transform.GetChild(0).GetComponent<Text>().text = value;
                    animButton.name = anims[i].name + "Btn";
                }

                animButton.GetComponent<Button>().onClick.AddListener(PlayAnim);
                // 만든 버튼을 버튼 리스트에 추가한다
                buttons.Add(animButton);
            }

            if (GameObject.Find("GameManager") != null) return;
            else
            {
                // 삭제하기 버튼을 만들어준다
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
                // 정말로 끄겠습니까 팝업은 꺼둔다
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
        //    // 다시 자기가 선택되면 애니메이션 버튼을 한번만 켜준다
        //    if (SH_EditorManager.Instance.activeObj == gameObject && active < 1)
        //    {
        //        // 막아주기
        //        if (buttons[0].gameObject.activeSelf == true) return;

        //        for (int i = 0; i < buttons.Count; i++)
        //        {
        //            buttons[i].gameObject.SetActive(true);
        //        }
        //        active++;
        //    }

        // 현재 선택된 오브젝트가 자기 자신이 아닌경우 애니메이션 버튼을 꺼준다
        if (objType.ToString() == "obj")
        {
            if (SH_EditorManager.Instance.activeObj != gameObject && buttons.Count > 0)
            {
                // 막아주기
                if (buttons[0].gameObject.activeSelf == false) return;

                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].gameObject.SetActive(false);
                    if (i == buttons.Count - 1)
                    {
                        // 정말 삭제하시겠습니까 팝업도 미리 꺼놔야한다
                        buttons[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }

                active = 0;
            }
        }
    }

    // 버튼을 클릭한대로 애니메이션이 재생된다
    public void PlayAnim()
    {
        // 선택된 버튼의 이름을 찾는다
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        // 버튼에 대한 이름을 찾아와서 그 애니메이션을 재생시킨다(이름 Dog_Walk)
        string clickName = clickBtn.name.Substring(0, clickBtn.name.Length - 3);
        string goName = gameObject.name.Substring(0, gameObject.name.Length - 7);
        anim.Play(goName + "_" + clickName);
        currentAnim = goName + "_" + clickName;
    }

    // 무조건 Delete버튼을 누르면 정말 종료하시겠습니까 팝업을 뜨게한다
    public void DeletePopUp()
    {
        GameObject popUp = delete.transform.GetChild(0).gameObject;
        if (popUp.activeSelf == true) popUp.SetActive(false);
        else popUp.SetActive(true);
    }

    // Delete 팝업 안에서 삭제하기 예를 누르면 게임오브젝트 파괴하고 
    // 아니오 누르면 팝업 자체를 끈다
    public void PopUpYes()
    {
        // 관련 버튼들도 모두 삭제해야한다
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
