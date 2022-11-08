using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_UIManager_MyRoom : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region 동물 선택 버튼
    public GameObject Animal;
    public void ChoiceAnimal()
    {
        Animal.SetActive(!Animal.activeSelf);
    }
    #endregion
}
