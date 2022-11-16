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

    #region ClickViewMore // 더보기 버튼 클릭
    public void ClickViewMore()
    {
        // 방금 클릭한 더보기 버튼을 가져오기
        addBtn = EventSystem.current.currentSelectedGameObject;
        // 더보기 버튼의 부모를 가져오기
        moreSection = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        RectTransform sectionTransform = moreSection.GetComponent<RectTransform> ();
        sectionTransform.SetHeight(500);
        // 텍스트 가져오기
        moreText = moreSection.transform.GetChild(0).gameObject;
        RectTransform textTreansform = moreText.GetComponent<RectTransform> ();
        //textTreansform.SetPosX(85);
        textTreansform.anchoredPosition = new Vector2(85, 0);

        // 두번째 줄부터의 오브젝트 나타내기
        GameObject.Find(moreSection.name).transform.Find("More").gameObject.SetActive(true);

        foreach (var section in sections)
        {
            // 만약 선택한 section이 아니라면 비활성화 처리
            if(section.gameObject != moreSection)
            {
                section.SetActive(false);
            }
        }
        addBtn.SetActive(false);
        backBtn.SetActive(true);
    }
    #endregion

    #region ClickBack // 뒤로가기 버튼 클릭
    public void ClickBack()
    {
        RectTransform sectionTransform = moreSection.GetComponent<RectTransform>();
        sectionTransform.SetHeight(100);

        RectTransform textTreansform = moreText.GetComponent<RectTransform>();
        textTreansform.SetPosX(45);

        // 두번째 줄부터의 오브젝트 숨기기
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
