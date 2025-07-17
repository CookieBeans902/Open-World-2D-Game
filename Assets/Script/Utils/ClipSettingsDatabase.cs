using UnityEngine;

[CreateAssetMenu(fileName = "SettingsList", menuName = "Scriptable Objects/SettingsList")]
public class ClipSettingsDatabase : ScriptableObject {
    public ClipSettings idle;
    public ClipSettings walk;
    public ClipSettings slash;
    public ClipSettings islash;
    public ClipSettings thrust;
    public ClipSettings smash;
    public ClipSettings shoot;
    public ClipSettings hurt;

    public ClipSettings Get(string key) {
        switch (key) {
            case "idle": return idle;
            case "walk": return walk;
            case "slash": return slash;
            case "islash": return islash;
            case "thrust": return thrust;
            case "smash": return smash;
            case "shoot": return shoot;
            case "hurt": return hurt;
            default:
                Debug.LogWarning($"ClipSettingsDatabase: Unknown key '{key}'");
                return null;
        }
    }

    public void Set(string key, ClipSettings value) {
        switch (key) {
            case "idle": idle = value; break;
            case "walk": walk = value; break;
            case "slash": slash = value; break;
            case "islash": islash = value; break;
            case "thrust": thrust = value; break;
            case "smash": smash = value; break;
            case "shoot": shoot = value; break;
            case "hurt": hurt = value; break;
            default:
                Debug.LogWarning($"ClipSettingsDatabase: Unknown key '{key}'");
                break;
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
