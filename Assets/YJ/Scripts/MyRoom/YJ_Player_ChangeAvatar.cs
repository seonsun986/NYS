using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YJ_Player_ChangeAvatar : MonoBehaviour
{
    // ĳ���� ����
    public GameObject[] avatar;

    // ��Ƽ���� ����
    public Material[] catMt;
    public Button[] catMtList;
    public Material[] bearMt;
    public Button[] bearMtList;
    public Material[] bunnyMt;
    public Button[] bunnyMtList;
    // ������Ʈ ����
    public GameObject[] obj;

    // ĳ����
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

    // Cat Mt ��ư
    public void OnClickCatMt(int i)
    {
        avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = catMt[i];
    }
}
