using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_RoomRay : MonoBehaviour
{
    // Ã³À½ À§Ä¡ÀúÀå
    void Start()
    {
        originPos = transform.position;
        originPosX = originPos.x;
    }

    Ray ray;
    RaycastHit hit;
    Vector3 originPos;
    float originPosX;
    bool change = false;
    bool hitPlayer = false;

            //BoxCast(transform.position, transform.localScale, -Vector3.up, out hit, Quaternion.identity, 10, 1<<2) && !change)
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(originPos, Vector3.down, out hit, 30, 1<<2) && !hitPlayer) 
        {
            Debug.DrawRay(transform.position, hit.point * 30, Color.red);
            print("¹¹ÀÖÂî?");
            
            hitPlayer = true;
            
        }



        if (hitPlayer && !change)
        {
            transform.localPosition += new Vector3(originPos.x + 3, originPos.y);

            float tranformX = transform.position.x;

            if (tranformX + originPosX > 3)
            {
                change = true;
            }
        }

        if (Physics.Raycast(originPos, Vector3.down, out hit, 30, 1 << 2) && hitPlayer && change)
        {
            hitPlayer = false;
            //change = false;
        }
    }
}
