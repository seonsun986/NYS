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

    int objBtn_1 = 9;
    public void Obj_BtnOff_0(int i)
    {
        // ������ ������ ��ȣ�� �ٸ� ���
        if (objBtn_1 != i && objBtn_1 != 9)
        {
            // ������ư�� �����������.....
            if (!OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().click)
            {
                // ���� ��ư�� �Ҵ�
                objBtn_1 = i;
                OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
            // ������ư�� �����������
            else
            {
                // ���� ��ư�� ����
                OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
                // ���� ��ư�� �Ҵ�
                objBtn_1 = i;
                OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
        }
        // ������ ������ ��ȣ�� ���
        else if (objBtn_1 == i)
        {
            OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
        // ó������ ��ư�� ���
        else if (objBtn_1 == 9)
        {
            objBtn_1 = i;
            OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
    }


    int objBtn_2 = 9;
    public void Obj_BtnOff_1(int i)
    {
        // ������ ������ ��ȣ�� �ٸ� ���
        if (objBtn_2 != i && objBtn_2 != 9)
        {
            // ������ư�� �����������.....
            if (!OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().click)
            {
                // ���� ��ư�� �Ҵ�
                objBtn_2 = i;
                OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
            // ������ư�� �����������
            else
            {
                // ���� ��ư�� ����
                OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
                // ���� ��ư�� �Ҵ�
                objBtn = i;
                OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
        }
        // ������ ������ ��ȣ�� ���
        else if (objBtn_2 == i)
        {
            OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
        // ó������ ��ư�� ���
        else if (objBtn_2 == 9)
        {
            objBtn_2 = i;
            OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
    }

    int objBtn_3 = 9;
    public void Obj_BtnOff_2(int i)
    {
        // ������ ������ ��ȣ�� �ٸ� ���
        if (objBtn_3 != i && objBtn_3 != 9)
        {
            // ������ư�� �����������.....
            if (!OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().click)
            {
                // ���� ��ư�� �Ҵ�
                objBtn_3 = i;
                OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
            // ������ư�� �����������
            else
            {
                // ���� ��ư�� ����
                OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
                // ���� ��ư�� �Ҵ�
                objBtn = i;
                OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
        }
        // ������ ������ ��ȣ�� ���
        else if (objBtn_3 == i)
        {
            OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
        // ó������ ��ư�� ���
        else if (objBtn_3 == 9)
        {
            objBtn_3 = i;
            OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
    }

    public void Obj_BtnOff_3(int i)
    {
        // ������ ������ ��ȣ�� �ٸ� ���
        if (objBtn != i && objBtn != 9)
        {
            // ������ư�� �����������.....
            if (!OBJButtons[objBtn].GetComponent<YJ_OutLine>().click)
            {
                // ���� ��ư�� �Ҵ�
                objBtn = i;
                OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
            // ������ư�� �����������
            else
            {
                // ���� ��ư�� ����
                OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
                // ���� ��ư�� �Ҵ�
                objBtn = i;
                OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
        }
        // ������ ������ ��ȣ�� ���
        else if (objBtn == i)
        {
            OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
        // ó������ ��ư�� ���
        else if (objBtn == 9)
        {
            objBtn = i;
            OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
    }
}
