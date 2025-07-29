using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ButtonScaling : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] RectTransform buttonTransform;
    Vector3 originalScale;
    [SerializeField] float scaleFactor = 0.8f;

    void Start()
    {
        buttonTransform = GetComponent<RectTransform>();
        originalScale = buttonTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonTransform.localScale = originalScale * scaleFactor; 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonTransform.localScale = originalScale; 
    }
}