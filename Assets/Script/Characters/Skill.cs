using UnityEngine;

public class Skill {
    public Element element;
    public Sprite icon;
    public string skillName;
    public string skillDesc;

    public int slot = -1;

    public static Skill Create(SkillSO skillSO) {
        if (skillSO == null) return null;

        return new Skill {
            element = skillSO.element,
            icon = skillSO.icon,
            skillName = skillSO.skillName,
            skillDesc = skillSO.skillDesc,
        };
    }
}