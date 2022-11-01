using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NK_FairyTale : MonoBehaviour
{
    public GameObject fairyTaleUI;
    public GameObject fairyTaleObject;
    public GameObject teacherUI;
    public GameObject childUI;
    public Transform book;
    public List<Transform> objs;

    Vector3 originalScale;

    // Start is called before the first frame update
    void OnEnable()
    {
        book.gameObject.SetActive(true);

        if (GameManager.Instance.photonView.IsMine)
        {
            // 태그에 따라 아이들 동화 플레이 UI, 선생님 동화 플레이 UI 띄움
            if (GameManager.Instance.photonView.gameObject.CompareTag("Child"))
            {
                childUI.SetActive(true);
            }
            if (GameManager.Instance.photonView.gameObject.CompareTag("Teacher"))
            {
                teacherUI.SetActive(true);
            }
        }

        //AddObjectList(fairyTaleUI);

        //AddObjectList(fairyTaleObject);
    }

    private void AddObjectList(GameObject fairyTaleContent)
    {
        foreach (Transform child in fairyTaleContent.GetComponentsInChildren<Transform>())
        {
            if (child.name == fairyTaleContent.name)
                continue;
            //child.localScale = new Vector3(0, 0, 0);
            if (child.GetComponent<Renderer>() != null)
            {
                Renderer ren = child.GetComponent<Renderer>();
                Color color = ren.material.color;
                print(color);
                color.a = 0;
                ren.material.color = color;
            }
            objs.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 책이 거의 다 펼쳐지면 활성화
        if (book.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            fairyTaleUI.SetActive(true);
            fairyTaleObject.SetActive(true);
        }

        // 크기 변형 (수정 중)
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].gameObject.activeSelf == true)
            {
                if (objs[i].transform.localScale.x < 1f)
                {
                    objs[i].transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }
    }

    private void OnDisable()
    {
        // 페어리테일 매니저가 꺼지면 UI, 오브젝트들 모두 꺼지도록
        fairyTaleUI.SetActive(false);
        fairyTaleObject.SetActive(false);
        book.gameObject.SetActive(false);

        if (GameManager.Instance.photonView.IsMine)
        {
            if (GameManager.Instance.photonView.gameObject.CompareTag("Child"))
            {
                childUI.SetActive(false);
            }
            if (GameManager.Instance.photonView.gameObject.CompareTag("Teacher"))
            {
                teacherUI.SetActive(false);
            }
        }
    }
}
