using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_FairyTale : MonoBehaviour
{
    public GameObject fairyTaleUI;
    public GameObject fairyTaleObject;
    public Transform book;
    public List<Transform> objs;

    Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        fairyTaleUI.SetActive(true);
        fairyTaleObject.SetActive(true);
        book.gameObject.SetActive(true);

        //AddObjectList(fairyTaleUI);

        //AddObjectList(fairyTaleObject);
    }

    private void AddObjectList(GameObject fairyTaleContent)
    {
        foreach (Transform child in fairyTaleContent.GetComponentsInChildren<Transform>())
        {
            if (child.name == fairyTaleContent.name)
                continue;
            //child.localScale = new Vector3(0, 0, 0);
            if (child.GetComponent<Renderer>() != null)
            {
                Renderer ren = child.GetComponent<Renderer>();
                Color color = ren.material.color;
                print(color);
                color.a = 0;
                ren.material.color = color;
            }
            objs.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].gameObject.activeSelf == true)
            {
                if (objs[i].transform.localScale.x < 1f)
                {
                    objs[i].transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            fairyTaleUI.SetActive(false);
            fairyTaleObject.SetActive(false);
            book.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
