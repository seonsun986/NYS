using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_RoomTextUI : MonoBehaviour
{
    Text text;

    void Start()
    {
        text = GetComponent<Text>();
        text.text = YJ_UIManager_Plaza.roomInfo.roomName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
