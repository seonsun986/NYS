using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page9 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject selectPopUp;
    public Animator mushroom;
    public Animator egg;
    public Animator spinach;
    public Animator rice;
    public Animator onion;
    public Animator potato;

    public GameObject mushroomText;
    public GameObject eggText;
    public GameObject spinachext;
    public GameObject onionText;
    public GameObject riceText;
    public GameObject potatoText;
    public YJ_ButtonClickSound sound;
    void Start()
    {
        
    }

    RaycastHit hitInfo;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray ,out hitInfo))
        {
            if (selectPopUp.activeSelf == true) return;
            if(hitInfo.transform.name == "MushroomBtn")
            {
#if UNITY_ANDROID
#else
                if(mushroom.enabled == false)
                {
                    mushroom.enabled = true;
                    mushroomText.SetActive(true);
                    egg.Rebind();
                    egg.enabled = false;
                    spinach.Rebind();
                    spinach.enabled = false;
                    rice.Rebind();
                    rice.enabled = false;
                    onion.Rebind();
                    onion.enabled = false;
                    potato.Rebind();
                    potato.enabled = false;
                    eggText.SetActive(false);
                    spinachext.SetActive(false);
                    riceText.SetActive(false);
                    onionText.SetActive(false);
                    potatoText.SetActive(false);
                }
#endif
                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                    sound.OnClickSound();
                }
            }


            else if (hitInfo.transform.name == "EggBtn")
            {
#if !UNITY_ANDROID
                if (egg.enabled == false)
                {
                    egg.enabled = true;
                    eggText.SetActive(true);
                    mushroom.Rebind();
                    mushroom.enabled = false;
                    spinach.Rebind();
                    spinach.enabled = false;
                    rice.Rebind();
                    rice.enabled = false;
                    onion.Rebind();
                    onion.enabled = false;
                    potato.Rebind();
                    potato.enabled = false;
                    mushroomText.SetActive(false);
                    spinachext.SetActive(false);
                    riceText.SetActive(false);
                    onionText.SetActive(false);
                    potatoText.SetActive(false);
                }
#endif

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                    sound.OnClickSound();

                }
            }


            else if (hitInfo.transform.name == "SpinachBtn")
            {
#if !UNITY_ANDROID

                if (spinach.enabled == false)
                {
                    spinach.enabled = true;
                    spinachext.SetActive(true);
                    mushroom.Rebind();
                    mushroom.enabled = false;
                    egg.Rebind();
                    egg.enabled = false;
                    rice.Rebind();
                    rice.enabled = false;
                    onion.Rebind();
                    onion.enabled = false;
                    potato.Rebind();
                    potato.enabled = false;
                    // ≈ÿΩ∫∆Æ ≤Ù±‚
                    eggText.SetActive(false);
                    mushroomText.SetActive(false);
                    riceText.SetActive(false);
                    onionText.SetActive(false);
                    potatoText.SetActive(false);
                }
#endif
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                    sound.OnClickSound();

                }
            }

            else if (hitInfo.transform.name == "RiceBtn")
            {
#if !UNITY_ANDROID
            if (rice.enabled == false)
                {
                    rice.enabled = true;
                    riceText.SetActive(true);
                    mushroom.Rebind();
                    mushroom.enabled = false;
                    spinach.Rebind();
                    spinach.enabled = false;
                    egg.Rebind();
                    egg.enabled = false;
                    onion.Rebind();
                    onion.enabled = false;
                    potato.Rebind();
                    potato.enabled = false;
                    eggText.SetActive(false);
                    spinachext.SetActive(false);
                    mushroomText.SetActive(false);
                    onionText.SetActive(false);
                    potatoText.SetActive(false);
                }
#endif
            if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                    sound.OnClickSound();

                }
            }

            else if (hitInfo.transform.name == "OnionBtn")
            {
#if !UNITY_ANDROID
            if (onion.enabled == false)
                {
                    onion.enabled = true;
                    onionText.SetActive(true);
                    mushroom.Rebind();
                    mushroom.enabled = false;
                    spinach.Rebind();
                    spinach.enabled = false;
                    egg.Rebind();
                    egg.enabled = false;
                    potato.Rebind();
                    potato.enabled = false;
                    rice.Rebind();
                    rice.enabled = false;

                    // ≈ÿΩ∫∆Æ ≤Ù±‚
                    eggText.SetActive(false);
                    spinachext.SetActive(false);
                    riceText.SetActive(false);
                    mushroomText.SetActive(false);
                    potatoText.SetActive(false);
                }
#endif
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                    sound.OnClickSound();

                }
            }

            else if (hitInfo.transform.name == "PotatoBtn")
            {
#if !UNITY_ANDROID
                if (potato.enabled == false)
                {
                    potato.enabled = true;
                    potatoText.SetActive(true);
                    mushroom.Rebind();
                    mushroom.enabled = false;
                    spinach.Rebind();
                    spinach.enabled = false;
                    egg.Rebind();
                    egg.enabled = false;
                    onion.Rebind();
                    onion.enabled = false;
                    rice.Rebind();
                    rice.enabled = false;

                    eggText.SetActive(false);
                    spinachext.SetActive(false);
                    riceText.SetActive(false);
                    onionText.SetActive(false);
                    mushroomText.SetActive(false);
                }
#endif
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                    sound.OnClickSound();

                }
            }
#if !UNITY_ANDROID
            else
            {
                mushroom.Rebind();
                mushroom.enabled = false;
                spinach.Rebind();
                spinach.enabled = false;
                egg.Rebind();
                egg.enabled = false;
                onion.Rebind();
                onion.enabled = false;
                rice.Rebind();
                rice.enabled = false;
                potato.Rebind();
                potato.enabled = false;

                // ≈ÿΩ∫∆Æ ≤Ù±‚
                potatoText.SetActive(false);
                eggText.SetActive(false);
                spinachext.SetActive(false);
                riceText.SetActive(false);
                onionText.SetActive(false);
                mushroomText.SetActive(false);
            }
#endif
        }
#if !UNITY_ANDROID
        else
        {
            mushroom.Rebind();
            mushroom.enabled = false;
            spinach.Rebind();
            spinach.enabled = false;
            egg.Rebind();
            egg.enabled = false;
            onion.Rebind();
            onion.enabled = false;
            rice.Rebind();
            rice.enabled = false;
            potato.Rebind();
            potato.enabled = false;

            // ≈ÿΩ∫∆Æ ≤Ù±‚
            potatoText.SetActive(false);
            eggText.SetActive(false);
            spinachext.SetActive(false);
            riceText.SetActive(false);
            onionText.SetActive(false);
            mushroomText.SetActive(false);

        }
#endif
    }
}
