using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform buttonTransform;
    private Vector3 originalScale;
    [SerializeField] float scale = 1.2f;

    void Start()
    {
        buttonTransform = GetComponent<RectTransform>();
        originalScale = buttonTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonTransform.localScale = originalScale*scale; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonTransform.localScale = originalScale; 
    }
}