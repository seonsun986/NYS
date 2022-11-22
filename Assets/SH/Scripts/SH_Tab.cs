using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Tab : MonoBehaviour
{
    public GameObject BG_Tab;
    public List<GameObject> scene_Tab = new List<GameObject>();
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowSound()
    {
        BG_Tab.SetActive(true);
        //for(int i =0; i < Scene_Tab.)
        for(int i=0;i< scene_Tab.Count;i++)
        {
            scene_Tab[i].SetActive(false);
        }
        BGImage.sprite = soundBG;
    }

    public Sprite pageBG;
    public Sprite soundBG;
    public Image BGImage;
    public void ShowSceneTab()
    {
        BG_Tab.SetActive(false);
        //Scene_Tab.SetActive(true);
        for (int i = 0; i < scene_Tab.Count; i++)
        {
            scene_Tab[i].SetActive(true);
        }
        BGImage.sprite = pageBG;
    }
}
