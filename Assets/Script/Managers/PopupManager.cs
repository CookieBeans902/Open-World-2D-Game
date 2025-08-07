using System;

using Game.Utils;

using UnityEngine;
using UnityEngine.UIElements;

public class PopupManager : MonoBehaviour {
    public static event Action<string, Vector2, float, Color> OnPopupRequest;
    [SerializeField] private Transform popupPref;

    private void OnEnable() {
        PopupManager.OnPopupRequest += ShowPopup;
    }

    private void OnDisable() {
        PopupManager.OnPopupRequest -= ShowPopup;
    }

    public static void RequestPopup(string val, Vector2 position, float size, Color color) {
        OnPopupRequest?.Invoke(val, position, size, color);
    }

    private void ShowPopup(string val, Vector2 position, float size, Color color) {
        Vector2 offset = new Vector2(0.5f, 0.5f);
        Popup popup = Instantiate(popupPref, position + offset, Quaternion.identity).GetComponent<Popup>();

        popup.text.text = val;
        popup.text.fontSize = size;
        popup.text.color = color;

        popup.canvas.overrideSorting = true;
        popup.canvas.sortingLayerName = "Popup";
        popup.canvas.sortingOrder = 100;

        FunctionTimer.CreateSceneTimer(() => Destroy(popup.gameObject), popup.clip.length);
    }
}
