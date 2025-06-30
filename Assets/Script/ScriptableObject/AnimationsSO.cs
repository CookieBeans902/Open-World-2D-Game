using UnityEngine;

[CreateAssetMenu(fileName = "AnimationsSO", menuName = "Scriptable Objects/Animations")]
public class AnimationsSO : ScriptableObject {
    public AnimationClip IdleDown;
    public AnimationClip IdleLeft;
    public AnimationClip IdleRight;
    public AnimationClip IdleUp;
    public AnimationClip MoveDown;
    public AnimationClip MoveLeft;
    public AnimationClip MoveRight;
    public AnimationClip MoveUp;
}
