using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class NK_DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

    RectTransform rectTransform;
    //CanvasGroup canvasGroup;
    [SerializeField] Canvas canvas;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        //canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //canvasGroup.alpha = .6f;
        //canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ������ ��ƼĿ�϶�
        if (gameObject.name.Contains("Clone"))
        {
            // ĵ������ �����ϰ� ����� �ϱ� ������
            // ���� �̵��� ���ؼ� �󸶳� �̵��ߴ����� ������
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //canvasGroup.alpha = 1f;
        //canvasGroup.blocksRaycasts = true;
        /*        for (int i = 0; i < NK_BookCover.instance.stickerList.Count; i++)
                {
                    if (NK_BookCover.instance.stickerList[i] != this.gameObject)
                    {
                        NK_BookCover.instance.stickerList.Add(this.gameObject);
                    }

                }*/
        //if (!NK_BookCover.instance.stickerList.Contains(this.gameObject))
        //{
        //    NK_BookCover.instance.stickerList.Add(this.gameObject);
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ������ ��ƼĿ�϶�
        if (gameObject.name.Contains("Clone"))
        {
            NK_BookCover.instance.delSticker = gameObject;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}