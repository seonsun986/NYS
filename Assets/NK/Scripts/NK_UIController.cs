using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_UIController : MonoBehaviour
{
    bool isMute = false;
    bool IsMute
    {
        get
        {
            if (isMute)
                isMute = false;
            else
                isMute = true;
            return isMute;
        }
    }

    bool isControl = false;
    bool IsControl
    {
        get
        {
            if (isControl)
                isControl = false;
            else
                isControl = true;
            return isControl;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region // 음소거 버튼
    public void ClickMute()
    {
        // 모든 아이들의 볼륨을 0으로 하거나 Mute 시킴
        for(int i = 0; i < GameManager.Instance.children.Count; i++)
        {
            AudioSource audio = GameManager.Instance.children[i].GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.mute = IsMute;
            }
        }
    }
    #endregion

    #region // 행동제어 버튼
    public void ClickControl()
    {
        // 모든 아이들을 가장 가까운 빈 좌석에 앉힘
        if(IsControl)
        {
            // 모든 좌석을 가져옴
            // 아이들과 좌석의 거리를 비교함
            // 가장 가까운 빈 좌석에 앉힘
        }
    }
    #endregion
}
