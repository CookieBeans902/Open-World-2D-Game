using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(Dialog))]
public class DialogLineDrawer : PropertyDrawer {
    // Store scroll positions and foldout state per property
    private static Dictionary<string, Vector2> scrollPositions = new Dictionary<string, Vector2>();
    private static Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        string key = property.propertyPath;

        if (!foldoutStates.ContainsKey(key))
            foldoutStates[key] = false;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        foldoutStates[key] = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, lineHeight),
            foldoutStates[key],
            label,
            true
        );

        if (!foldoutStates[key])
            return;

        SerializedProperty id = property.FindPropertyRelative("id");
        SerializedProperty speaker = property.FindPropertyRelative("speaker");
        SerializedProperty text = property.FindPropertyRelative("text");

        EditorGUI.indentLevel++;

        float y = position.y + lineHeight + 2;

        Rect idLabelRect = new Rect(position.x, y, (position.width - spacing) / 2, lineHeight);
        Rect speakerLabelRect = new Rect(position.x + (position.width + spacing) / 2, y, (position.width - spacing) / 2, lineHeight);
        EditorGUI.LabelField(idLabelRect, "id");
        EditorGUI.LabelField(speakerLabelRect, "speaker");

        Rect idRect = new Rect(position.x, y + lineHeight + spacing, (position.width - spacing) / 2, lineHeight);
        Rect speakerRect = new Rect(position.x + (position.width + spacing) / 2, y + lineHeight + spacing, (position.width - spacing) / 2, lineHeight);
        EditorGUI.PropertyField(idRect, id, GUIContent.none);
        EditorGUI.PropertyField(speakerRect, speaker, GUIContent.none);

        y += lineHeight + 4;

        if (!scrollPositions.ContainsKey(key))
            scrollPositions[key] = Vector2.zero;

        Vector2 scroll = scrollPositions[key];

        Rect scrollViewRect = new Rect(position.x, y + lineHeight + spacing, position.width, 90);
        float textHeight = GUI.skin.textArea.CalcHeight(new GUIContent(text.stringValue), scrollViewRect.width - 20);
        textHeight = Mathf.Max(100, textHeight);
        Rect contentRect = new Rect(0, 0, scrollViewRect.width - 20, textHeight);

        scroll = GUI.BeginScrollView(scrollViewRect, scroll, contentRect);
        text.stringValue = EditorGUI.TextArea(new Rect(0, 0, contentRect.width, contentRect.height), text.stringValue, EditorStyles.textArea);
        GUI.EndScrollView();

        scrollPositions[key] = scroll;
        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        string key = property.propertyPath;

        if (!foldoutStates.ContainsKey(key) || !foldoutStates[key])
            return EditorGUIUtility.singleLineHeight;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        float totalHeight = 0;
        totalHeight += lineHeight;
        totalHeight += spacing + lineHeight;
        totalHeight += spacing + lineHeight;
        totalHeight += spacing + 90;
        totalHeight += spacing;

        return totalHeight;
    }
}
