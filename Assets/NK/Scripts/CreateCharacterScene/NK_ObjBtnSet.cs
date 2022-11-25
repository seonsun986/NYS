using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_ObjBtnSet : MonoBehaviour
{
    bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        YJ_ButtonSetOff.instance.SetObjBtn();
        isStart = true;
    }

    private void OnEnable()
    {
        if (isStart)
        {
            YJ_ButtonSetOff.instance.SetObjBtn();
        }
    }
}
