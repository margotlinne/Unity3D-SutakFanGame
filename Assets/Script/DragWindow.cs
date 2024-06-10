using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private Canvas canvas;
    private Image bgImage;
    private Vector3 orgPos;
    private Color orgColor;
    private Color bgColor;


    void Awake()
    {
        bgImage = dragRectTransform.GetComponent<Image>();
        // start 함수 내에서 설정하니까 ondisable이 먼저 호출되는건지 값이 자꾸 0,0으로 설정되었음
        orgPos = dragRectTransform.localPosition;
    }
    void Start()
    {
        orgColor = bgImage.color;
        Debug.Log(orgPos);
    }
    void OnDisable()
    {
        dragRectTransform.anchoredPosition = orgPos;   
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        bgColor = orgColor;
        bgColor.a = .9f;
        bgImage.color = bgColor;
        dragRectTransform.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bgImage.color = orgColor;
    }
}
