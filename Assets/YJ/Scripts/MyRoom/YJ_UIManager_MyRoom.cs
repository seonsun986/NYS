using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YJ_UIManager_MyRoom : MonoBehaviour
{
    public GameObject T_BookBtn;
    public GameObject T_BookBtn2;
    public GameObject C_BookBtn;

    void Start()
    {
        loading.SetActive(false);
        if (YJ_DataManager.instance.myInfo.memberRole == "TEACHER")
        {
            C_BookBtn.SetActive(false);
        }
        else
        {
            T_BookBtn.SetActive(false);
            T_BookBtn2.SetActive(false);
        }
    }

    void Update()
    {

    }

    #region 아바타 꾸미기 버튼
    // 아바타 꾸미기 버튼
    public void OnClickAvatar()
    {
        SceneManager.LoadScene("CreateCharacter");
    }
    #endregion

    #region 책만들기 버튼
    public void OnClickCreateBook()
    {
        loading.SetActive(true);
        StartCoroutine("Loading");
    }
    #endregion

    #region 로딩 코루틴
    public GameObject loading;
    IEnumerator Loading()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("EditorScene");

        while (!asyncOperation.isDone)
        {
            yield return null;
            loading.transform.GetChild(1).GetComponent<Image>().fillAmount += asyncOperation.progress;
        }
    }
    #endregion

    #region 학생 책 선택 버튼
    public void StudentBook()
    {
        SceneManager.LoadScene("EditorChildren");
    }
    #endregion

    #region 선생님 책 선택 버튼
    public void TeacherBook()
    {
        SceneManager.LoadScene("BookShelfScene");
    }
    #endregion
}
