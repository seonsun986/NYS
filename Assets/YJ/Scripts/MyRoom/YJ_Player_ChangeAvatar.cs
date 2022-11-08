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
        //avt = avatar[(int.Parse(UserInfo.animal))];
        avt = avatar[0];
        avt.SetActive(true);
        //avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = catMt[(int.Parse(UserInfo.material))];
        avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = catMt[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int j = 0;
    public void OnClickCatMt()
    {
        GameObject catMtChoice = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i <= catMtList.Length; i++)
        {
            if (catMtList[i].gameObject.name == catMtChoice.name)
            {
                j = i;
                avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = catMt[j];
            }
            
        }
    }
}
