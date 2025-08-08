using UnityEngine;

public class LvlUpEffect : MonoBehaviour {
    [SerializeField] private AnimationClip clip;
    private float elapsed = 0;

    private void Update() {
        if (elapsed < clip.length - 0.06f) {
            elapsed += Time.deltaTime;
        }
        else {
            elapsed = 0;
            PlayerMove move = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMove>();
            if (move != null) move.EnableMovement();
            Destroy(gameObject);
        }
    }
}
