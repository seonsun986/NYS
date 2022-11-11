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
        }
    }
}
