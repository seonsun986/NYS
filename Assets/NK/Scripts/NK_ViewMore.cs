using Photon.Voice.Unity.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NK_ViewMore : MonoBehaviour
{
    public GameObject moreSection;
    public GameObject moreText;
    public GameObject addBtn;
    public GameObject backBtn;
    public List<GameObject> sections = new List<GameObject> ();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region ClickViewMore // ������ ��ư Ŭ��
    public void ClickViewMore()
    {
        // ��� Ŭ���� ������ ��ư�� ��������
        addBtn = EventSystem.current.currentSelectedGameObject;
        // ������ ��ư�� �θ� ��������
        moreSection = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        RectTransform sectionTransform = moreSection.GetComponent<RectTransform> ();
        sectionTransform.SetHeight(500);
        // �ؽ�Ʈ ��������
        moreText = moreSection.transform.GetChild(0).gameObject;
        RectTransform textTreansform = moreText.GetComponent<RectTransform> ();
        //textTreansform.SetPosX(85);
        textTreansform.anchoredPosition = new Vector2(85, 0);

        // �ι�° �ٺ����� ������Ʈ ��Ÿ����
        GameObject.Find(moreSection.name).transform.Find("More").gameObject.SetActive(true);

        foreach (var section in sections)
        {
            // ���� ������ section�� �ƴ϶�� ��Ȱ��ȭ ó��
            if(section.gameObject != moreSection)
            {
                section.SetActive(false);
            }
        }
        addBtn.SetActive(false);
        backBtn.SetActive(true);
    }
    #endregion

    #region ClickBack // �ڷΰ��� ��ư Ŭ��
    public void ClickBack()
    {
        RectTransform sectionTransform = moreSection.GetComponent<RectTransform>();
        sectionTransform.SetHeight(100);

        RectTransform textTreansform = moreText.GetComponent<RectTransform>();
        textTreansform.SetPosX(45);

        // �ι�° �ٺ����� ������Ʈ �����
        GameObject.Find(moreSection.name).transform.Find("More").gameObject.SetActive(false);

        foreach (var section in sections)
        {
            section.SetActive(true);
        }
        addBtn.SetActive(true);
        backBtn.SetActive(false);
    }
    #endregion
}
