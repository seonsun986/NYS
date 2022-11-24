using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_AvatarBtnSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        YJ_ButtonSetOff.instance.SetAvatarBtn();
    }
}
