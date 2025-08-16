using UnityEngine;

public class SlideUp : MonoBehaviour
{
    public GameObject panelToOpen;
    public float animTime = 1f;
    [SerializeField] float screenDisplaceFactor = 0.75f;
    void Awake()
    {
        panelToOpen.transform.localPosition = new Vector2(0, -Screen.height*screenDisplaceFactor);
    }
    public void OnOpen()
    {
        panelToOpen.LeanMoveLocalY(0, animTime).setEaseOutCubic();
    }

    public void OnClose()
    {
        panelToOpen.LeanMoveLocalY(-Screen.height*screenDisplaceFactor, animTime).setEaseInCubic().setOnComplete(OnComplete);
    }
    public void OnComplete()
    {
        GameUIManager.Instance.CloseUI(panelToOpen);
    }
}
