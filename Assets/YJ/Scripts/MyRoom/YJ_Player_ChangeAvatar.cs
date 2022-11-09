using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YJ_Player_ChangeAvatar : MonoBehaviour
{
    // 캐릭터 정보
    public GameObject[] avatar;

    // 머티리얼 정보
    public Material[] catMt;
    public Material[] bearMt;
    public Material[] bunnyMt;
    // 오브젝트 정보
    public GameObject[] obj;

    // 캐릭터
    GameObject avt;
    // Start is called before the first frame update
    void Start()
    {
        avt = avatar[0];
        avt.SetActive(true);
        avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = catMt[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickAvatar(int i)
    {
        avt.SetActive(false);
        avt = avatar[i];
        avt.SetActive(true);
    }


    // Cat Mt 버튼
    public void OnClickCatMt(int i)
    {
        avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = catMt[i];
    }
}
