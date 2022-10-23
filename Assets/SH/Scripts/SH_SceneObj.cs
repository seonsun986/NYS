using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SH_SceneObj : MonoBehaviour
{
    // 빈 오브젝트 안에 들어가야하는 것들 : InputField, Gameobject
    // 이때 InputField는 canvas안에 다시 들어가야한다'
    // 게임오브젝트는 빈 오브젝트 자식으로 넣지만 InputField같은 경우에는 Canvas에서 올려야하는데
    int mySceneNum;
    public enum ObjType
    {
        text,
        obj,
    }

    public ObjType objType;
    void Start()
    {
        if(GetComponent<SH_InputField>())
        {
            objType = ObjType.text;
        }
        else
        {
            objType = ObjType.obj;
        }
        // 내가 속해 있는 Scene 번호를 저장한다
        mySceneNum = SH_BtnManager.Instance.i;
        print(objType);
    }

    void Update()
    {
        
    }
}
