using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_BtnManager : MonoBehaviour
{
    public Image sceneBG;
    public Image objectBG;
    public RectTransform SceneBtn;
    public RectTransform ObjectBtn;
    Animation sceneAnim;
    Animation objectAnim;
    bool sceneBG_view;
    bool objectBG_view;

    void Start()
    {
        sceneAnim = sceneBG.GetComponent<Animation>();
        objectAnim = objectBG.GetComponent<Animation>();
    }

    void Update()
    {
        
    }


    #region 에디터 배경 나오기 버튼 함수들
    public void MoveSceneBG()
    {
        // sceneBG가 보이지 않는다면
        if(sceneBG_view == false)
        {
            sceneAnim.Play("SceneBGShowAnim");
            SceneBtn.rotation = new Quaternion(0, 0, -180,0);       // 버튼 돌려주기
            sceneBG_view = true;
        }
        // 그렇지 않다면
        else
        {
            sceneAnim.Play("SceneBGShowOffAnim");
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);

            sceneBG_view = false;
        }
    }

    public void MoveObjectBG()
    {
        // objectBG가 보이지 않는다면
        if (objectBG_view == false)
        {
            objectAnim.Play("ObjectBGShowAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, 0, 0);       // 버튼 돌려주기
            objectBG_view = true;
        }

        //그렇지 않다면
        else
        {
            objectAnim.Play("ObjectBGShowOffAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, -180, 0);    
            objectBG_view = false;
        }
    }
    #endregion

}
