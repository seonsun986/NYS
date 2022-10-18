using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NK_TabButton : MonoBehaviour
{
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    // �ش� ��ư�� ���õǾ��ִ��� ����
    public bool isSelect;

    public void Select()
    {
        if(onTabSelected != null)
        {
            isSelect = true;
            onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if(onTabDeselected != null)
        {
            isSelect = false;
            onTabDeselected.Invoke();
        }
    }

    public void OnSelectedTab(NK_TabButton button)
    {
        NK_TabController.Instance.SelectedButton(button);
    }
}
