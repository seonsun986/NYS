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

    // � ���� ��������� ���� ���͸��� �ٸ��� �������
    int animNum;
    public void OnClickAnimalSet(int i)
    {
        animNum = i;
    }

    // Animal��ư
    public GameObject animalList;
    public void OnClickAnimal()
    {
        MatList[animNum].SetActive(false);
        objList.SetActive(false);
        animalList.SetActive(!animalList.activeSelf);
    }

    // Mat��ư
    public GameObject[] MatList;
    public void OnClickMat()
    {
        animalList.SetActive(false);
        objList.SetActive(false);
        MatList[animNum].SetActive(!MatList[animNum].activeSelf);
    }

    // Obj��ư
    public GameObject objList;
    public void OnClickObj()
    {
        animalList.SetActive(false);
        MatList[animNum].SetActive(false);
        objList.SetActive(!objList.activeSelf);
    }

    // �ڷΰ���
    public void OnClickBackBTN()
    {
        SceneManager.LoadScene("MyRoomScene");
    }
}
