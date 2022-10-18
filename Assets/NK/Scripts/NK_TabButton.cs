using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NK_TabButton : MonoBehaviour
{
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    public void Select()
    {
        if(onTabSelected != null)
            onTabSelected.Invoke();
    }

    public void Deselect()
    {
        if(onTabDeselected != null)
            onTabDeselected.Invoke();
    }

    public void OnSelectedTab(NK_TabButton button)
    {
        NK_TabController.Instance.SelectedButton(button);
    }
}
