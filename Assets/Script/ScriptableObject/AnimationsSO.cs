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

    public AnimationClip SlashDown;
    public AnimationClip SlashLeft;
    public AnimationClip SlashRight;
    public AnimationClip SlashUp;

    public AnimationClip ISlashDown;
    public AnimationClip ISlashLeft;
    public AnimationClip ISlashRight;
    public AnimationClip ISlashUp;

    public AnimationClip ThrustDown;
    public AnimationClip ThrustLeft;
    public AnimationClip ThrustRight;
    public AnimationClip ThrustUp;

    public AnimationClip ShootDown;
    public AnimationClip ShootLeft;
    public AnimationClip ShootRight;
    public AnimationClip ShootUp;
}
