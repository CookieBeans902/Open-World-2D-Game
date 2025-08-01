using UnityEngine;

[CreateAssetMenu(fileName = "WalkAnimationNameSO", menuName = "Scriptable Objects/WalkAnimationNameSO")]
public class WalkAnimationNameSO : ScriptableObject {
    public string IdleDown = "idle_down";
    public string IdleLeft = "idle_left";
    public string IdleRight = "idle_right";
    public string IdleUp = "idle_up";

    public string WalkDown = "walk_down";
    public string WalkLeft = "walk_left";
    public string WalkRight = "walk_right";
    public string WalkUp = "walk_up";
}
