using UnityEngine;

public class PlayerShared : MonoBehaviour {
    public Transform visual;
    public Transform healthbar;
    public Collider2D pCollider;
    public Vector3 playerDir;
    public float baseMoveSpeed = 8;
    public float moveSpeed = 8;


    private void Start() {
        playerDir = Vector2.right;
    }
}
