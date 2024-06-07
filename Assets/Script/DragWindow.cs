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
    }
    void Start()
    {
        orgPos = dragRectTransform.localPosition;
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
