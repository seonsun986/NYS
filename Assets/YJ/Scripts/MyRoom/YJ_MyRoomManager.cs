using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YJ_MyRoomUIManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 아바타 꾸미기 버튼
    void OnClickAvatar()
    {
        SceneManager.LoadScene("CreateCharacter");
    }
}
