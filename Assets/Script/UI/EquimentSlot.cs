using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour {
    public Image icon;
    public Button button;
    public Animator animator;
    public GameObject selected;

    public UnityEvent onSingleClick = new UnityEvent();
    public UnityEvent onDoubleClick = new UnityEvent();
    public float doubleClickThreshold = 0.3f;
    private float lastClickTime = -1f;

    private void Start() {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick() {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickThreshold) {
            onDoubleClick?.Invoke();
            lastClickTime = -1f;
        }
        else {
            lastClickTime = Time.time;
            onSingleClick.Invoke();
        }
    }
}
