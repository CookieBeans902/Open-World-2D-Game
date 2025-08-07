using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform buttonTransform;
    private Vector3 originalScale;
    [SerializeField] float scale = 1.2f;
    AudioManager Audio;

    void Start()
    {
        buttonTransform = GetComponent<RectTransform>();
        originalScale = buttonTransform.localScale;
        Audio = AudioManager.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonTransform.localScale = originalScale * scale;
        Audio.ButtonSFX(Audio.buttonHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonTransform.localScale = originalScale;
        Audio.ButtonSFX(Audio.buttonHover);
    }
    public void SFX()
    {
        Audio.ButtonSFX(Audio.buttonClick);
    }

}