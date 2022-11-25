using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YJ_UIManager_MyRoom : MonoBehaviour
{
    public GameObject popupBG;
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

    #region 환경설정
    public GameObject settingSet;
    //int settingCount = 0;

    public void Setting()
    {
        popupBG.SetActive(!settingSet.activeSelf);
        settingSet.SetActive(!settingSet.activeSelf);
        bgmOnOff.isOn = YJ_AudioManager.instance.bgmOnOff;
        effectOnOff.isOn = YJ_AudioManager.instance.effectOnOff;
    }
    #endregion

    #region BGM On/Off
    public Toggle bgmOnOff;
    public GameObject bgmOn, bgmOff;
    // 228, 291
    // 배경음악 On/Off
    public void BGMOnAndOff()
    {
        if (bgmOnOff.isOn)
        {
            YJ_AudioManager.instance.bgmOnOff = true;
            //Camera.main.GetComponent<AudioSource>().volume = 1;
            bgmOff.SetActive(false);
            bgmOn.SetActive(true);
        }
        else
        {
            YJ_AudioManager.instance.bgmOnOff = false;
            //Camera.main.GetComponent<AudioSource>().volume = 0;
            bgmOn.SetActive(false);
            bgmOff.SetActive(true);
        }
    }
    #endregion

    #region EffectSound On/Off
    public Toggle effectOnOff;
    public GameObject effectOn, effectOff;

    GameObject canvas;

    // 228, 291
    // 배경음악 On/Off
    public void EffectOnAndOff()
    {
        canvas = GameObject.Find("Canvas");
        if (effectOnOff.isOn)
        {
            YJ_AudioManager.instance.effectOnOff = true;
            //canvas.GetComponent<AudioSource>().volume = 1;
            effectOff.SetActive(false);
            effectOn.SetActive(true);
        }
        else
        {
            YJ_AudioManager.instance.effectOnOff = false;
            //canvas.GetComponent<AudioSource>().volume = 0;
            effectOn.SetActive(false);
            effectOff.SetActive(true);
        }
    }
    #endregion

    #region 게임종료버튼

    public void GameExit()
    {
        Application.Quit();
    }

    #endregion
}
