using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YJ_UIManager_Avatar : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 어떤 동물 골랐는지에 따라 메터리얼 다르게 띄워야함
    int animNum;
    public void OnClickAnimalSet(int i)
    {
        animNum = i;
    }

    // Animal버튼
    public GameObject animalList;
    public void OnClickAnimal()
    {
        MatList[animNum].SetActive(false);
        objList.SetActive(false);
        animalList.SetActive(!animalList.activeSelf);
    }

    // Mat버튼
    public GameObject[] MatList;
    public void OnClickMat()
    {
        animalList.SetActive(false);
        objList.SetActive(false);
        MatList[animNum].SetActive(!MatList[animNum].activeSelf);
    }

    // Obj버튼
    public GameObject objList;
    public void OnClickObj()
    {
        animalList.SetActive(false);
        MatList[animNum].SetActive(false);
        objList.SetActive(!objList.activeSelf);
    }

    // 뒤로가기
    public void OnClickBackBTN()
    {
        SceneManager.LoadScene("MyRoomScene");
    }
}
