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

    #region �ƹ�Ÿ �ٹ̱� ��ư
    // �ƹ�Ÿ �ٹ̱� ��ư
    public void OnClickAvatar()
    {
        SceneManager.LoadScene("CreateCharacter");
    }
    #endregion

    #region å����� ��ư
    public void OnClickCreateBook()
    {
        loading.SetActive(true);
        StartCoroutine("Loading");
    }
    #endregion

    #region �ε� �ڷ�ƾ
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

    #region �л� å ���� ��ư
    public void StudentBook()
    {
        SceneManager.LoadScene("EditorChildren");
    }
    #endregion

    #region ������ å ���� ��ư
    public void TeacherBook()
    {
        SceneManager.LoadScene("BookShelfScene");
    }
    #endregion
}
