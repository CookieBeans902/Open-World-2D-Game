using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteChangePress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Sprite pressedSprite;
    [SerializeField] Sprite defaultSprite;
    Image image;
    void Awake()
    {
        image = gameObject.GetComponent<Image>();
        defaultSprite = image.sprite;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = pressedSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = defaultSprite;
    }
}
