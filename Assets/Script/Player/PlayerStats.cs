using UnityEngine;

public class PlayerStats : MonoBehaviour, IStats {
    private PlayerShared shared;
    private float mhp = 100;
    private float hp = 100;
    private void Start() {
        shared = GetComponent<PlayerShared>();
    }

    public void TakeDamage(float atk, float luck) {
        hp -= atk;
        hp = Mathf.Clamp(hp, 0, mhp);
        shared.healthbar.SetFill(hp / mhp);
    }
}
