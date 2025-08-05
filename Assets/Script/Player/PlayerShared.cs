using UnityEngine;

public class PlayerShared : MonoBehaviour {
    public Transform visual;
    public Healthbar healthbar;
    public Rigidbody2D rb;
    public Collider2D collider2d;
    public Animator animator;

    public Movement move;
    public Animations anim;
    public PlayerInteractions interact;
    public PlayerAttack attack;

    public int charId;


    private void Start() {
        move = GetComponent<Movement>();
        anim = GetComponent<Animations>();
        interact = GetComponent<PlayerInteractions>();
        attack = GetComponent<PlayerAttack>();
    }
}
