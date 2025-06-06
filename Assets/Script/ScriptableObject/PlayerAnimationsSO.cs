using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimationsSO", menuName = "Scriptable Objects/PlayerAnimations")]
public class PlayerAnimationsSO : ScriptableObject {
    public AnimationClip IdleDown;
    public AnimationClip IdleLeft;
    public AnimationClip IdleRight;
    public AnimationClip IdleUp;
    public AnimationClip MoveDown;
    public AnimationClip MoveLeft;
    public AnimationClip MoveRight;
    public AnimationClip MoveUp;
}
