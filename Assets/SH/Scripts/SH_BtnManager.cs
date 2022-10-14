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


    #region ������ ��� ������ ��ư �Լ���
    public void MoveSceneBG()
    {
        // sceneBG�� ������ �ʴ´ٸ�
        if(sceneBG_view == false)
        {
            sceneAnim.Play("SceneBGShowAnim");
            SceneBtn.rotation = new Quaternion(0, 0, -180,0);       // ��ư �����ֱ�
            sceneBG_view = true;
        }
        // �׷��� �ʴٸ�
        else
        {
            sceneAnim.Play("SceneBGShowOffAnim");
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);

            sceneBG_view = false;
        }
    }

    public void MoveObjectBG()
    {
        // objectBG�� ������ �ʴ´ٸ�
        if (objectBG_view == false)
        {
            objectAnim.Play("ObjectBGShowAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, 0, 0);       // ��ư �����ֱ�
            objectBG_view = true;
        }

        //�׷��� �ʴٸ�
        else
        {
            objectAnim.Play("ObjectBGShowOffAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, -180, 0);    
            objectBG_view = false;
        }
    }
    #endregion

}
