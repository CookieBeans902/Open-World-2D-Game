using Pathfinding;
using UnityEngine;

public class Minimap : MonoBehaviour {
    private GridGraph grid;
    private GameObject playerFollowCamera;

    private void Start() {
        grid = AstarPath.active.data.gridGraph;
        playerFollowCamera = GameObject.FindWithTag("PlayerFollowCamera");
    }
}
