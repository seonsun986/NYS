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

    // �ƹ�Ÿ �ٹ̱� ��ư
    void OnClickAvatar()
    {
        SceneManager.LoadScene("CreateCharacter");
    }
}
