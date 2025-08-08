using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour {
    public TextMeshProUGUI text;
    public Canvas canvas;
    public Image icon;

    [SerializeField] private AnimationClip clip;
    private float elapsed;

    private void Update() {
        if (elapsed < clip.length - 0.05f)
            elapsed += Time.deltaTime;
        else
            Destroy(gameObject);
    }
}
