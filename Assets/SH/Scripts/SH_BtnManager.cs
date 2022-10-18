using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// InputField 필요한 정보들
public class TextInfo
{
    public string inputs { get; set; }
    public int txtDropdown { get; set; }
    public int txtSize { get; set; }
}

public class SH_BtnManager : MonoBehaviour
{
    public Image sceneBG;
    public Image objectBG;
    public RectTransform SceneBtn;
    public RectTransform ObjectBtn;
    Animation sceneAnim;
    Animation objectAnim;
    bool sceneBG_view;
    bool objectBG_view;

    public GameObject inputField;       // inputField 프리팹
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // 현재 선택되어있는 드롭다운과 텍스트 사이즈
    public Dropdown txtDropdown;
    public Text txtSize;

    // 씬 추가하기
    public GameObject voidScene;
    public GameObject rawImage;

    void Start()
    {
        sceneAnim = sceneBG.GetComponent<Animation>();
        objectAnim = objectBG.GetComponent<Animation>();
    }

    void Update()
    {
        
    }


    #region 에디터 배경 나오기 버튼 함수들
    public void MoveSceneBG()
    {
        // sceneBG가 보이지 않는다면       
        if(sceneBG_view == false)
        {
            sceneAnim.Play("SceneBGShowAnim");            
            SceneBtn.rotation = new Quaternion(0, 0, -180,0);       // 버튼 돌려주기
            sceneBG_view = true;
            // 이때 objectBG가 보인다면
            if(objectBG_view == true)
            {
                objectAnim.Play("ObjectBGShowOffAnim");
                ObjectBtn.rotation = new Quaternion(0, 0, -180, 0);
                objectBG_view = false;
            }
        }
        // 그렇지 않다면
        else
        {
            sceneAnim.Play("SceneBGShowOffAnim");
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
            sceneBG_view = false;
        }
    }

    public void MoveObjectBG()
    {
        // objectBG가 보이지 않는다면
        if (objectBG_view == false)
        {
            objectAnim.Play("ObjectBGShowAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, 0, 0);       // 버튼 돌려주기
            objectBG_view = true;
            // 이때 SceneBG가 보인다면
            if(sceneBG_view == true)
            {
                sceneAnim.Play("SceneBGShowOffAnim");
                SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
                sceneBG_view = false;
            }
        }

        //그렇지 않다면
        else
        {
            objectAnim.Play("ObjectBGShowOffAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, -180, 0);    
            objectBG_view = false;
        }
    }
    #endregion


    // 텍스트 추가 함수
    // 텍스트를 추가할 때 처음엔 기본 값으로 세팅한다
    // 드롭다운이 변형될 때마다 폰트를 변경한다
    // 텍스트 사이즈를 변경할 때마다 폰트 크기를 변경한다
    // 변경할 때마다의 값을 각자만의 클래스에 저장해놓는다.
    // 해당 함수는 시작할 때만 값을 할당한다
    public void AddText()
    {
        SH_InputField inputText = Instantiate(inputField).GetComponent<SH_InputField>();
        inputText.info = new TextInfo
        {
            inputs = inputText.GetComponent<InputField>().text,
            txtDropdown = txtDropdown.value,
            txtSize = int.Parse(txtSize.text),
        };
        // 선택되어있는 dropdown과 textSize값에 따라서 글자 크기를 바꾸기 위함
        inputFields.Add(inputText);
        inputText.transform.SetParent(GameObject.Find("Canvas").transform);
        inputText.transform.localPosition = new Vector3(0, 0, 0);

    }

    public void PlusSize()
    {
        int size = int.Parse( txtSize.text);
        size++;
        txtSize.text = size.ToString();
    }

    public void MinusSize()
    {
        int size = int.Parse(txtSize.text);
        size--;
        txtSize.text = size.ToString();
    }


    // 씬 추가하기 함수
    // Save를 누르거나, 씬을 추가하는 순간 해당 부분을 캡쳐한다.
    int i = 0;
    public void AddScene()
    {
        GameObject scene = Instantiate(voidScene);
        GameObject raw = Instantiate(rawImage);
        raw.transform.SetParent(sceneBG.transform);
        scene.transform.SetParent(GameObject.Find("CanvasForScene").transform);
        scene.transform.position = new Vector3(0, 0.5f, 0);
        i++;
        print(i);
    }

}
