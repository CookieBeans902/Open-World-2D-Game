using UnityEngine;

public class SlideUp : MonoBehaviour
{
    public GameObject panelToOpen;
    public float animTime = 0.5f;
    void Awake()
    {
        panelToOpen.transform.localPosition = new Vector2(0, -Screen.height/2);
    }
    public void OnOpen()
    {
        panelToOpen.LeanMoveLocalY(0, animTime).setEaseOutCubic();
    }

    public void OnClose()
    {
        panelToOpen.LeanMoveLocalY(-Screen.height/2, animTime).setEaseInCubic().setOnComplete(OnComplete);
    }
    public void OnComplete()
    {
        GameUIManager.Instance.CloseUI(panelToOpen);
    }
}
