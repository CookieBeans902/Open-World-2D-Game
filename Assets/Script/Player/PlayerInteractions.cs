using System;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour {
    // To get the shared data for player gameobject
    private PlayerShared shared;
    private GameInputManager input;

    private void Start() {
        shared = GetComponent<PlayerShared>();
        input = GameInputManager.Instance;
    }

    private void Update() {
        if (input.IsInteracting()) CheckInteractions();
    }


    /// <summary>TO check if there is any interactable object infront of the player</summary>
    private void CheckInteractions() {
        Vector2 dir = shared.playerDir;
        float r = 2;
        int layerMask = ~LayerMask.GetMask("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, r, layerMask);
        if (hit.collider != null) {
            InteractionBase target = hit.collider.GetComponent<InteractionBase>();
            if (target != null) {
                target.Interact();
            }
        }
    }
}
