using UnityEngine;

public class PopUp : MonoBehaviour {
    public GameObject panelToOpen;
    [SerializeField] bool open;
    public float animTime = 0.5f;
    void Awake() {
        panelToOpen.transform.localScale = Vector2.zero;
    }
    public void OnEnable() {
        if(!open) panelToOpen.LeanScale(Vector2.one, animTime).setEaseOutCubic().setDelay(0.1f);
        open = true;
    }

    public void OnClose()
    {
        open = false;
        panelToOpen.LeanScale(Vector2.zero, animTime).setEaseInBack().setOnComplete(OnComplete);
    }
    void OnComplete()
    {
        GameUIManager.Instance.CloseUI(panelToOpen);
    }
}
