using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NK_LoadPreview : MonoBehaviour
{
    public void LoadPreview()
    {
        SceneManager.LoadScene("PreviewScene");
    }

    public void LoadEditor()
    {
        SceneManager.LoadScene("EditorScene");
    }
}
