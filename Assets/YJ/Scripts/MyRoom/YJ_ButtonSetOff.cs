using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_ButtonSetOff : MonoBehaviour
{
    public Button[] animalButtons;
    public int animalBtn = 4;
    public Button[] cat_MTButtons;
    public int catMtBtn = 9;
    public Button[] bear_MTButtons;
    public int bearMtBtn = 9;
    public Button[] rabbit_MTButtons;
    public int rabbitMtBtn = 9;
    public Button[] OBJButtons;
    public int objBtn = 9;

    public static YJ_ButtonSetOff instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetAvatarBtn()
    {
        animalBtn = YJ_Player_ChangeAvatar.instance.avatarNum;
        animalButtons[animalBtn].GetComponent<YJ_OutLine>().OnClickImageChange();
    }

    public void SetMeshBtn(int avatar)
    {
        switch (avatar)
        {
            // 아바타에 따라 버튼 세팅
            case 0:
                catMtBtn = YJ_Player_ChangeAvatar.instance.setmt;
                cat_MTButtons[catMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
                break;
            case 1:
                bearMtBtn = YJ_Player_ChangeAvatar.instance.setmt;
                bear_MTButtons[bearMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
                break;
            case 2:
                rabbitMtBtn = YJ_Player_ChangeAvatar.instance.setmt;
                rabbit_MTButtons[rabbitMtBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
                break;
        }
    }

    public void SetObjBtn()
    {
        // 초기화
        if (objBtn_1 < 9)
        {
            OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
            objBtn_1 = 9;
        }
        // 아바타 세팅에 가방 있으면
        if (YJ_Player_ChangeAvatar.instance.bagId < 8)
        {
            objBtn_1 = YJ_Player_ChangeAvatar.instance.bagId;
            OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
        }

        if (objBtn_2 < 9)
        {
            OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
            objBtn_2 = 9;
        }
        if (YJ_Player_ChangeAvatar.instance.fishId < 8)
        {
            objBtn_2 = YJ_Player_ChangeAvatar.instance.fishId;
            OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
        }

        if (objBtn_3 < 9)
        {
            OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
            objBtn_3 = 9;
        }
        if (YJ_Player_ChangeAvatar.instance.hatId < 8)
        {
            objBtn_3 = YJ_Player_ChangeAvatar.instance.hatId;
            OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
        }

        if (objBtn < 9)
        {
            OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
            objBtn = 9;
        }
        if (YJ_Player_ChangeAvatar.instance.glassId < 8)
        {
            objBtn = YJ_Player_ChangeAvatar.instance.glassId;
            OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
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
        else if (bearMtBtn == i)
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
        // 이전에 선택한 번호와 다른 경우
        if (objBtn_1 != i && objBtn_1 != 9)
        {
            // 이전버튼이 꺼져있을경우.....
            if (!OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().click)
            {
                // 지금 버튼을 켠다
                objBtn_1 = i;
                OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
            // 이전버튼이 켜져있을경우
            else
            {
                // 이전 버튼을 끄고
                OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
                // 지금 버튼을 켠다
                objBtn_1 = i;
                OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
        }
        // 이전에 선택한 번호일 경우
        else if (objBtn_1 == i)
        {
            OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
        // 처음누른 버튼일 경우
        else if (objBtn_1 == 9)
        {
            objBtn_1 = i;
            OBJButtons[objBtn_1].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
    }


    int objBtn_2 = 9;
    public void Obj_BtnOff_1(int i)
    {
        // 이전에 선택한 번호와 다른 경우
        if (objBtn_2 != i && objBtn_2 != 9)
        {
            // 이전버튼이 꺼져있을경우.....
            if (!OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().click)
            {
                // 지금 버튼을 켠다
                objBtn_2 = i;
                OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
            // 이전버튼이 켜져있을경우
            else
            {
                // 이전 버튼을 끄고
                OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
                // 지금 버튼을 켠다
                objBtn_2 = i;
                OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
        }
        // 이전에 선택한 번호일 경우
        else if (objBtn_2 == i)
        {
            OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
        // 처음누른 버튼일 경우
        else if (objBtn_2 == 9)
        {
            objBtn_2 = i;
            OBJButtons[objBtn_2].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
    }

    int objBtn_3 = 9;
    public void Obj_BtnOff_2(int i)
    {
        // 이전에 선택한 번호와 다른 경우
        if (objBtn_3 != i && objBtn_3 != 9)
        {
            // 이전버튼이 꺼져있을경우.....
            if (!OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().click)
            {
                // 지금 버튼을 켠다
                objBtn_3 = i;
                OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
            // 이전버튼이 켜져있을경우
            else
            {
                // 이전 버튼을 끄고
                OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
                // 지금 버튼을 켠다
                objBtn_3 = i;
                OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
        }
        // 이전에 선택한 번호일 경우
        else if (objBtn_3 == i)
        {
            OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
        // 처음누른 버튼일 경우
        else if (objBtn_3 == 9)
        {
            objBtn_3 = i;
            OBJButtons[objBtn_3].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
    }

    public void Obj_BtnOff_3(int i)
    {
        // 이전에 선택한 번호와 다른 경우
        if (objBtn != i && objBtn != 9)
        {
            // 이전버튼이 꺼져있을경우.....
            if (!OBJButtons[objBtn].GetComponent<YJ_OutLine>().click)
            {
                // 지금 버튼을 켠다
                objBtn = i;
                OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
            // 이전버튼이 켜져있을경우
            else
            {
                // 이전 버튼을 끄고
                OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
                // 지금 버튼을 켠다
                objBtn = i;
                OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
            }
        }
        // 이전에 선택한 번호일 경우
        else if (objBtn == i)
        {
            OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
        // 처음누른 버튼일 경우
        else if (objBtn == 9)
        {
            objBtn = i;
            OBJButtons[objBtn].GetComponent<YJ_OutLine>().OnClickMTChange();
        }
    }
}
