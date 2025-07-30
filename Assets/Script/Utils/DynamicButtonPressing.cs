using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DynamicButtonPressing : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ISelectHandler,IDeselectHandler
{
    [SerializeField] Image image;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Transform pressedTransform;
    [SerializeField] Transform notPressedTransform;
    [SerializeField] bool style;
    void Start()
    {
        image.sprite = defaultSprite;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!style)
        {
            image.sprite = defaultSprite;
            buttonText.gameObject.transform.position = notPressedTransform.position;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!style)
        {
            image.sprite = selectedSprite;
            buttonText.gameObject.transform.position = pressedTransform.position;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (style)
        {
            image.sprite = selectedSprite;
            buttonText.gameObject.transform.position = pressedTransform.position;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (style)
        {
            image.sprite = defaultSprite;
            buttonText.gameObject.transform.position = notPressedTransform.position;
        }
    }
}
