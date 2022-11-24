using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_ButtonSetOff : MonoBehaviour
{
    public Button[] animalButtons;
    int animalBtn = 4;
    public Button[] cat_MTButtons;
    int catMtBtn = 9;
    public Button[] bear_MTButtons;
    int bearMtBtn = 9;
    public Button[] rabbit_MTButtons;
    int rabbitMtBtn = 9;
    public Button[] OBJButtons;
    int objBtn = 9;

    void Start()
    {
        
    }

    public void AnimalBtnOff(int i)
    {
        if (animalBtn == 4)
        {
            animalBtn = i;
            animalButtons[animalBtn].GetComponent<YJ_OutLine>().OnClickImageChange();
        }

        if (animalBtn != 4 && animalBtn != i)
        {
            animalButtons[animalBtn].GetComponent<YJ_OutLine>().OnClickImageChange();
            animalBtn = i;
        }
        else if (animalBtn == i)
            animalButtons[animalBtn].GetComponent<YJ_OutLine>().OnClickImageChange();
    }

    public void Cat_MtBtnOff(int i)
    {
        if (catMtBtn == 9)
        {
            catMtBtn = i;
            cat_MTButtons[catMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
        }

        if (catMtBtn != 9 && catMtBtn != i)
        {
            cat_MTButtons[catMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
            catMtBtn = i;
        }
        else if (catMtBtn == i)
            cat_MTButtons[catMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();

    }

    public void Bear_MtBtnOff(int i)
    {
        if (bearMtBtn == 9)
        {
            bearMtBtn = i;
            bear_MTButtons[bearMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
        }

        if (bearMtBtn != 9 && bearMtBtn != i)
        {
            bear_MTButtons[bearMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
            bearMtBtn = i;
        }
        else if(bearMtBtn == i)
            bear_MTButtons[bearMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();

    }

    public void Rabbit_MtBtnOff(int i)
    {
        if (rabbitMtBtn == 9)
        {
            rabbitMtBtn = i;
            rabbit_MTButtons[rabbitMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
        }

        if (rabbitMtBtn != 9 && rabbitMtBtn != i)
        {
            rabbit_MTButtons[rabbitMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
            rabbitMtBtn = i;
        }
        else if (rabbitMtBtn == i)
        {
            rabbit_MTButtons[rabbitMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
    }

    public void Obj_BtnOff(int i)
    {
        if (objBtn == 9)
        {
            objBtn = i;
            OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickImageChange();
        }

        if (objBtn != 9 && objBtn != i)
        {
            OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickImageChange();
            objBtn = i;
        }
        else if(objBtn == i)
            OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickImageChange();
    }
}
