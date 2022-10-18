using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// InputField �ʿ��� ������
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

    public GameObject inputField;       // inputField ������
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // ���� ���õǾ��ִ� ��Ӵٿ�� �ؽ�Ʈ ������
    public Dropdown txtDropdown;
    public Text txtSize;

    // �� �߰��ϱ�
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


    #region ������ ��� ������ ��ư �Լ���
    public void MoveSceneBG()
    {
        // sceneBG�� ������ �ʴ´ٸ�       
        if(sceneBG_view == false)
        {
            sceneAnim.Play("SceneBGShowAnim");            
            SceneBtn.rotation = new Quaternion(0, 0, -180,0);       // ��ư �����ֱ�
            sceneBG_view = true;
            // �̶� objectBG�� ���δٸ�
            if(objectBG_view == true)
            {
                objectAnim.Play("ObjectBGShowOffAnim");
                ObjectBtn.rotation = new Quaternion(0, 0, -180, 0);
                objectBG_view = false;
            }
        }
        // �׷��� �ʴٸ�
        else
        {
            sceneAnim.Play("SceneBGShowOffAnim");
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
            sceneBG_view = false;
        }
    }

    public void MoveObjectBG()
    {
        // objectBG�� ������ �ʴ´ٸ�
        if (objectBG_view == false)
        {
            objectAnim.Play("ObjectBGShowAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, 0, 0);       // ��ư �����ֱ�
            objectBG_view = true;
            // �̶� SceneBG�� ���δٸ�
            if(sceneBG_view == true)
            {
                sceneAnim.Play("SceneBGShowOffAnim");
                SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
                sceneBG_view = false;
            }
        }

        //�׷��� �ʴٸ�
        else
        {
            objectAnim.Play("ObjectBGShowOffAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, -180, 0);    
            objectBG_view = false;
        }
    }
    #endregion


    // �ؽ�Ʈ �߰� �Լ�
    // �ؽ�Ʈ�� �߰��� �� ó���� �⺻ ������ �����Ѵ�
    // ��Ӵٿ��� ������ ������ ��Ʈ�� �����Ѵ�
    // �ؽ�Ʈ ����� ������ ������ ��Ʈ ũ�⸦ �����Ѵ�
    // ������ �������� ���� ���ڸ��� Ŭ������ �����س��´�.
    // �ش� �Լ��� ������ ���� ���� �Ҵ��Ѵ�
    public void AddText()
    {
        SH_InputField inputText = Instantiate(inputField).GetComponent<SH_InputField>();
        inputText.info = new TextInfo
        {
            inputs = inputText.GetComponent<InputField>().text,
            txtDropdown = txtDropdown.value,
            txtSize = int.Parse(txtSize.text),
        };
        // ���õǾ��ִ� dropdown�� textSize���� ���� ���� ũ�⸦ �ٲٱ� ����
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


    // �� �߰��ϱ� �Լ�
    // Save�� �����ų�, ���� �߰��ϴ� ���� �ش� �κ��� ĸ���Ѵ�.
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
