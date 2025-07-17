using UnityEngine;

public class PlayerShared : MonoBehaviour {
    public Transform visual;
    public Transform healthbar;
    public Rigidbody2D rb;
    public Collider2D collider2d;
    public Animator animator;

    public Movement playerMove;
    public Animations playerAnim;
    public PlayerInteractions playerInteract;
    public PlayerAttack attack;


    private void Start() {
        playerMove = GetComponent<Movement>();
        playerAnim = GetComponent<Animations>();
        playerInteract = GetComponent<PlayerInteractions>();
        attack = GetComponent<PlayerAttack>();
    }
}
