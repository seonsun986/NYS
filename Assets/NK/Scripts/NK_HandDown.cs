using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_HandDown : MonoBehaviour
{
    private void OnDisable()
    {
        NK_UIController.instance.HandDown();
    }
}
