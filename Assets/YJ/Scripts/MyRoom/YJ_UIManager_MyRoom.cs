using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YJ_UIManager_MyRoom : MonoBehaviour
{
    void Start()
    {
        
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
}
