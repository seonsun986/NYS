using Photon.Voice.Unity.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NK_ViewMore : MonoBehaviour
{
    public GameObject moreSection;
    public List<GameObject> sections = new List<GameObject> ();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewMore()
    {
        moreSection = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        RectTransform rectTransform = moreSection.GetComponent<RectTransform> ();
        rectTransform.SetHeight(500);

        foreach (var section in sections)
        {
            if(section.gameObject != moreSection)
            {
                section.SetActive(false);
            }
        }
    }
}
