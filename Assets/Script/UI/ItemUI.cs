using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    public InventoryItem item;
    private Canvas canvas;
    private Canvas parentCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public bool isActive;
    public bool goBack;
    private Vector2 startPos;

    private void Start() {
        canvas = GetComponent<Canvas>();
        parentCanvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        // transform.SetParent(parentCanvas.transform, false);
        goBack = true;
        startPos = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / (canvas.scaleFactor != 0 ? canvas.scaleFactor : 1);
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        if (goBack) {
            // transform.SetParent(parentCanvas.transform, false);
            rectTransform.anchoredPosition = startPos;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        StatsUIManager.Instance.ShowItemDesc(item);
        // Debug.Log("Pointer Down");
    }
}
