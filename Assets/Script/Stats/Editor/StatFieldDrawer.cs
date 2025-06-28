using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(StatField<int, int>))]
public class StatFieldDrawerIntInt : PropertyDrawer {
    // Store foldout states
    private static Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Drawer.DrawGUI(position, property, label, ref foldouts);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return Drawer.GetProperties(property, label, foldouts);
    }
}


[CustomPropertyDrawer(typeof(StatField<int, float>))]
public class StatFieldDrawerIntFloat : PropertyDrawer {
    // Store foldout states
    private static Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Drawer.DrawGUI(position, property, label, ref foldouts);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return Drawer.GetProperties(property, label, foldouts);
    }
}

[CustomPropertyDrawer(typeof(StatField<float, int>))]
public class StatFieldDrawerFloatInt : PropertyDrawer {
    // Store foldout states
    private static Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Drawer.DrawGUI(position, property, label, ref foldouts);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return Drawer.GetProperties(property, label, foldouts);
    }
}

[CustomPropertyDrawer(typeof(StatField<float, float>))]
public class StatFieldDrawerFloatFloat : PropertyDrawer {
    // Store foldout states
    private static Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Drawer.DrawGUI(position, property, label, ref foldouts);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return Drawer.GetProperties(property, label, foldouts);
    }
}

class Drawer {
    public static void DrawGUI(Rect position, SerializedProperty property, GUIContent label, ref Dictionary<string, bool> foldouts) {
        string key = property.propertyPath;
        if (!foldouts.ContainsKey(key)) foldouts[key] = false;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        // Draw foldout
        Rect foldoutRect = new Rect(position.x, position.y, position.width, lineHeight);
        foldouts[key] = EditorGUI.Foldout(foldoutRect, foldouts[key], label, true);

        if (!foldouts[key]) return;

        // Indent content
        EditorGUI.indentLevel++;

        // Get properties
        var initProp = property.FindPropertyRelative("init");
        var finalProp = property.FindPropertyRelative("final");
        var powProp = property.FindPropertyRelative("pow");

        // Calculate layout
        float fieldWidth = (position.width - 2 * spacing) / 3f;
        float labelY = position.y + lineHeight + spacing;
        float fieldY = labelY + lineHeight + spacing / 2;

        // Draw labels
        EditorGUI.LabelField(new Rect(position.x, labelY, fieldWidth, lineHeight), "Init");
        EditorGUI.LabelField(new Rect(position.x + fieldWidth + spacing, labelY, fieldWidth, lineHeight), "Final");
        EditorGUI.LabelField(new Rect(position.x + 2 * (fieldWidth + spacing), labelY, fieldWidth, lineHeight), "Pow");

        // Draw fields
        EditorGUI.PropertyField(
            new Rect(position.x, fieldY, fieldWidth, lineHeight), initProp, GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(position.x + fieldWidth + spacing, fieldY, fieldWidth, lineHeight), finalProp, GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(position.x + 2 * (fieldWidth + spacing), fieldY, fieldWidth, lineHeight), powProp, GUIContent.none);

        EditorGUI.indentLevel--;
    }

    public static float GetProperties(SerializedProperty property, GUIContent label, Dictionary<string, bool> foldouts) {
        string key = property.propertyPath;
        if (foldouts.ContainsKey(key) && foldouts[key]) {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            return lineHeight + spacing + lineHeight + spacing + lineHeight; // foldout + label + fields
        }
        else {
            return EditorGUIUtility.singleLineHeight; // Only the foldout
        }
    }
}
