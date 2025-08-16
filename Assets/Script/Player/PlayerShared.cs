using UnityEngine;

public class PlayerShared : MonoBehaviour {
    public Transform visual;
    public Transform lvlUpEffect;
    public Healthbar healthbar;
    public Rigidbody2D rb;
    public Collider2D collider2d;
    public Animator animator;

    public PlayerMove move;
    public Animations anim;
    public PlayerInteractions interact;
    public PlayerAttack attack;
    public float attackSpeedFactor = 1;
    public float moveSpeedFactor = 1;

    public int charId;
    private float elapsed = 0;
    private float atkBuffTime;


    private void Start() {
        move = GetComponent<PlayerMove>();
        anim = GetComponent<Animations>();
        interact = GetComponent<PlayerInteractions>();
        attack = GetComponent<PlayerAttack>();
    }

    private void Update() {
        if (attackSpeedFactor != 1) {
            if (elapsed < atkBuffTime) {
                elapsed += Time.deltaTime;
            }
            else {
                attackSpeedFactor = 1;
                elapsed = 0;
            }
        }
    }

    public void SetAtkSpeedBuff(int amount) {
        attackSpeedFactor = (float)amount / 100;
        attackSpeedFactor = Mathf.Clamp(attackSpeedFactor, 1, 6);

        elapsed = 0;
        atkBuffTime = 2;
    }
}
