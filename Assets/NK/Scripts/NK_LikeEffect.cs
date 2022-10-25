using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_LikeEffect : MonoBehaviourPun
{
    public float holdingTime = 1;
    float currentTime;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        // ��Ʈ �����ǰ� 1�� �� Destroy
        if(currentTime > holdingTime)
        {
            Destroy(gameObject);
            currentTime = 0;
        }
    }
}
