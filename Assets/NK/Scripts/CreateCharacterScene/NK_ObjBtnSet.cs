using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_ObjBtnSet : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        YJ_ButtonSetOff.instance.SetObjBtn();
    }
}
