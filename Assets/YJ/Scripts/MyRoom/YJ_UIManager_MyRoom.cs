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

    #region �ƹ�Ÿ �ٹ̱� ��ư
    // �ƹ�Ÿ �ٹ̱� ��ư
    public void OnClickAvatar()
    {
        SceneManager.LoadScene("CreateCharacter");
    }
    #endregion
}
