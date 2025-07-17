using UnityEngine;

public class PlayerStats : MonoBehaviour, IStats {
    [SerializeField] private Healthbar healthbar;
    private float mhp = 100;
    private float hp = 100;
    public void TakeDamage(float atk, float luck) {
        hp -= atk;
        hp = Mathf.Clamp(hp, 0, mhp);
        healthbar.SetFill(hp / mhp);
    }
}
