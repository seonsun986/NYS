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
        // 생성된 스티커일때
        if (gameObject.name.Contains("Clone"))
        {
            // 캔버스의 스케일과 맞춰야 하기 때문에
            // 이전 이동과 비교해서 얼마나 이동했는지를 보여줌
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
        // 생성된 스티커일때
        if (gameObject.name.Contains("Clone"))
        {
            NK_BookCover.instance.delSticker = gameObject;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}