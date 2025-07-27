using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Scriptable Objects/SkillSO")]
public class SkillSO : ScriptableObject {
    public Element element;
    public Sprite icon;
    public string skillName;
    [TextArea]
    public string skillDesc;
}
