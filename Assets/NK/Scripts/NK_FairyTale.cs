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
    public GameObject bookBtn;
    public GameObject handupBtn;
    public Transform book;
    public List<Transform> objs;
    public GameObject stage;
    public AudioSource bgSound;
    public GameObject popupBG;

    // Start is called before the first frame update
    void OnEnable()
    {
        popupBG.SetActive(false);
        book.gameObject.SetActive(true);
        NK_UIController.instance.ClickControl(true);

        if (GameManager.Instance.photonView.IsMine)
        {
            // 태그에 따라 아이들 동화 플레이 UI, 선생님 동화 플레이 UI 띄움
            if (GameManager.Instance.photonView.gameObject.CompareTag("Child"))
            {
                childUI.SetActive(true);
                handupBtn.SetActive(true);
            }
            if (GameManager.Instance.photonView.gameObject.CompareTag("Teacher"))
            {
                teacherUI.SetActive(true);
                bookBtn.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 책이 거의 다 펼쳐지면 UI, 무대, 오브젝트들, 배경음 활성화
        if (book.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            stage.SetActive(true);
            fairyTaleUI.SetActive(true);
            fairyTaleObject.SetActive(true);
            bgSound.Pause();
        }
    }

    private void OnDisable()
    {
        // 페어리테일 매니저가 꺼지면 UI, 무대, 오브젝트들, 배경음 모두 꺼지도록
        fairyTaleUI.SetActive(false);
        fairyTaleObject.SetActive(false);
        book.gameObject.SetActive(false);
        stage.SetActive(false);
        bgSound.Play();

        if (GameManager.Instance.photonView.IsMine)
        {
            if (GameManager.Instance.photonView.gameObject.CompareTag("Child"))
            {
                childUI.SetActive(false);
                handupBtn.SetActive(false);
            }
            if (GameManager.Instance.photonView.gameObject.CompareTag("Teacher"))
            {
                teacherUI.SetActive(false);
                bookBtn.SetActive(true);
            }
        }
    }
}
