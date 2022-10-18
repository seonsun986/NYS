using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_TabController : MonoBehaviour
{
    private static NK_TabController instance;

    public static NK_TabController Instance
    {
        get
        {
            if (instance == null)
                GameObject.FindObjectOfType<NK_TabController>();
            return instance;
        }
    }

    NK_TabButton tabButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectedButton(NK_TabButton button)
    {
        // 이미 버튼이 선택되었다면
        if(button.isSelect)
        {
            tabButton.Deselect();
        }
        else
        {
            // 다른 버튼을 선택했을 때
            if (tabButton != null)
            {
                tabButton.Deselect();
            }
            tabButton = button;
            tabButton.Select();
        }
    }
}
