using UnityEngine;

public class RaycastTest : MovementBase {
    private Transform player;
    [SerializeField] private string searchTag;
    [SerializeField] private string blockTag;

    private void Update() {
        FindPlayer();
    }
    private void FindPlayer() {
        Vector3 start = transform.position;
        if (Input.GetKeyDown(KeyCode.L)) {
            Vector3 dir = Vector2.left;
            Debug.Log(CheckForTarget(start, dir, 120, 6, searchTag, blockTag));
        }
        else if (Input.GetKeyDown(KeyCode.R)) {
            Vector3 dir = Vector2.right;
            Debug.Log(CheckForTarget(start, dir, 120, 6, searchTag, blockTag));
        }
        else if (Input.GetKeyDown(KeyCode.U)) {
            Vector3 dir = Vector2.up;
            Debug.Log(CheckForTarget(start, dir, 120, 6, searchTag, blockTag));
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            Vector3 dir = Vector2.down;
            Debug.Log(CheckForTarget(start, dir, 120, 6, searchTag, blockTag));
        }
    }
}
