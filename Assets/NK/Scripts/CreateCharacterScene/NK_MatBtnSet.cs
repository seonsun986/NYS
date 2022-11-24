using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_MatBtnSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (YJ_Player_ChangeAvatar.instance.setmt < 8)
        {
            YJ_ButtonSetOff.instance.SetMeshBtn(YJ_Player_ChangeAvatar.instance.avatarNum);
        }
    }
}
