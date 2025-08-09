using UnityEngine;

public class ScaleUp : MonoBehaviour {
    public GameObject panelToOpen;
    public CanvasGroup background;
    public bool inactiveParent = false;
    public float animTime = 0.5f;
    void Awake() {
        background.alpha = 0;
        panelToOpen.transform.localScale = Vector2.zero;
    }
    public void OnEnable() {
        background.LeanAlpha(1, animTime);
        panelToOpen.LeanScale(Vector2.one, animTime).setEaseOutCubic().setDelay(0.1f);
    }

    public void OnClose() {
        panelToOpen.LeanScale(Vector2.zero, animTime).setEaseInBack().setOnComplete(OnComplete);
        background.LeanAlpha(0, animTime).setDelay(0.1f);
    }
    void OnComplete()
    {
        GameUIManager.Instance.CloseUI(panelToOpen.transform.parent.gameObject);
        GameUIManager.Instance.CloseUI(panelToOpen.gameObject);
    }
}
