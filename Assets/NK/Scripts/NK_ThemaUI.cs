using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class NK_ThemaUI : MonoBehaviour
{
    public enum Thema
    {
        Concept1,
        Concept2,
        Concept3,
    }

    public Thema selectedThema = Thema.Concept1;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelectThema(Thema thema)
    {
        selectedThema = thema;
        print(thema);
    }

    public void ClickThema1()
    {
        SelectThema(Thema.Concept1);
    }
    
    public void ClickThema2()
    {
        SelectThema(Thema.Concept2);
    }
    
    public void ClickThema3()
    {
        SelectThema(Thema.Concept3);
    }
}
