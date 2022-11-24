using Newtonsoft.Json.Linq;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_AvatarSet : MonoBehaviourPun
{
    // 캐릭터 정보
    public GameObject[] avatar;

    // 머티리얼 정보
    public Material[,] avtMat;
    public Material[] catMt;
    public Material[] bearMt;
    public Material[] bunnyMt;
    // 오브젝트 정보
    public GameObject[] obj;

    public GameObject avt;
    protected Transform bagParent;
    protected Transform fishParent;
    protected Transform hatParent;
    protected Transform glassParent;
    protected GameObject crown;

    public UserInfo userInfo = new UserInfo();

    //Test

    public virtual void Start()
    {
        avtMat = new Material[avatar.Length, catMt.Length];
        for (int i = 0; i < catMt.Length; i++)
            avtMat[0, i] = catMt[i];
        for (int i = 0; i < bearMt.Length; i++)
            avtMat[1, i] = bearMt[i];
        for (int i = 0; i < bunnyMt.Length; i++)
            avtMat[2, i] = bunnyMt[i];
    }

    public int bagId = 8;
    public int fishId = 8;
    public int hatId = 8;
    public int glassId = 8;
    protected void AvtSet()
    {
        avt = avatar[int.Parse(userInfo.animal)];
        avt.SetActive(true);

        SetItemParent();
        print(avt);
        print(avtMat);
        //몸체
        avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = avtMat[int.Parse(userInfo.animal), int.Parse(userInfo.material)];

        print("적용끝");

        //objectName(Json정보) 정보를 Parsing 해야한다.
        JObject jsonData = JObject.Parse(userInfo.objectName);
        bagId = jsonData["bag"].ToObject<int>();
        fishId = jsonData["fish"].ToObject<int>();
        hatId = jsonData["hat"].ToObject<int>();
        glassId = jsonData["glass"].ToObject<int>();
        

        if (bagId != 8)
        {
            Instantiate(obj[bagId], bagParent);
        }

        if (fishId != 8)
        {
            Instantiate(obj[fishId], fishParent);
        }

        if (hatId != 8)
        {
            Instantiate(obj[hatId], hatParent);
        }

        if (glassId != 8)
        {
            Instantiate(obj[glassId], glassParent);
        }

        if (userInfo.memberRole == "TEACHER")
        {
            crown.SetActive(true);
        }
    }

    protected void SetItemParent()
    {
        bagParent = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.transform;
        fishParent = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(4).gameObject.transform;
        hatParent = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.transform;
        glassParent = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.transform;
        crown = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject;
    }
}
