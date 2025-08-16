using TMPro;

using UnityEngine;

public class DamagePopup : MonoBehaviour {
    public TextMeshProUGUI text;
    public Canvas canvas;

    [SerializeField] private AnimationClip clip;
    private float elapsed;

    private void Update() {
        if (elapsed < clip.length - 0.05f)
            elapsed += Time.deltaTime;
        else
            Destroy(gameObject);
    }
}
