using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NK_ManageUI : MonoBehaviourPun
{
    public GameObject childListFactory;
    public Transform childrenListContent;

    void OnEnable()
    {
        foreach (PhotonView child in GameManager.Instance.children)
        {
            Text nickname = childListFactory.transform.GetChild(1).GetComponent<Text>();
            nickname.text = child.Owner.NickName;
            GameObject childList = GameObject.Instantiate(childListFactory);
            childList.transform.parent = childrenListContent;
            childList.transform.localScale = Vector3.one;
            print(child.Owner.NickName);
        }
    }

    private void OnDisable()
    {
        // child 에는 부모와 자식이 함께 설정 된다.
        var child = childrenListContent.GetComponentsInChildren<Transform>();

        foreach (var iter in child)
        {
            // 부모(this.gameObject)는 삭제 하지 않기 위한 처리
            if (iter != childrenListContent.transform)
            {
                Destroy(iter.gameObject);
            }
        }
    }
}
