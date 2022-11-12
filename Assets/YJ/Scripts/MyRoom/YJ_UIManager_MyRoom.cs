using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YJ_UIManager_MyRoom : MonoBehaviour
{
    public GameObject T_BookBtn;
    public GameObject C_BookBtn;

    void Start()
    {
        if (YJ_DataManager.instance.myInfo.memberRole == "TEACHER")
        {
            C_BookBtn.SetActive(false);
        }
        else
        {
            T_BookBtn.SetActive(false);
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
        SceneManager.LoadScene("EditorScene");
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
