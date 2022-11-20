using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page9 : MonoBehaviour
{
    // Start is called before the first frame update
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
    void Start()
    {
        
    }

    RaycastHit hitInfo;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray ,out hitInfo))
        {
            if(hitInfo.transform.name == "MushroomBtn")
            {
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

                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                }
            }


            else if (hitInfo.transform.name == "EggBtn")
            {
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

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                }
            }


            else if (hitInfo.transform.name == "SpinachBtn")
            {
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

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                }
            }

            else if (hitInfo.transform.name == "RiceBtn")
            {
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

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                }
            }

            else if (hitInfo.transform.name == "OnionBtn")
            {
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

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                }
            }

            else if (hitInfo.transform.name == "PotatoBtn")
            {
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

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.FillEmptyBox();
                }
            }

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

        }

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
    }
}
