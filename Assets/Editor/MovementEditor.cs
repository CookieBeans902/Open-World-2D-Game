using UnityEditor;

using UnityEngine;
using Game.Utils;

[CustomEditor(typeof(MBase))]
public class MovementEditor : Editor {
    public override void OnInspectorGUI() {
        MBase move = (MBase)target;

        move.moveType = (MoveType)EditorGUILayout.EnumPopup("move Type", move.moveType);

        switch (move.moveType) {
            case MoveType.Random:
                SerializeRandom();
                break;

            case MoveType.Chase:
                SerializeChase();
                break;

            case MoveType.Patrol:
                move.pathFromVectors = EditorGUILayout.Toggle("Path From Vectors", move.pathFromVectors);

                SerializePatrol(move.pathFromVectors);
                break;

            case MoveType.RunAway:
                SerializeRunAway();
                break;

            case MoveType.Follow:
                move.sameAsPlayerSpeed = EditorGUILayout.Toggle("Same As PlayerSpeed", move.sameAsPlayerSpeed);

                SerializeFollow(move.sameAsPlayerSpeed);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void SerializeRandom() {
        SerializedProperty restingSpeed = serializedObject.FindProperty("restingSpeed");
        EditorGUILayout.PropertyField(restingSpeed);

        SerializedProperty restingRadius = serializedObject.FindProperty("restingRadius");
        EditorGUILayout.PropertyField(restingRadius);
        EditorGUILayout.HelpBox("Set to 0 for unbounded radius", MessageType.None);

        SerializedProperty waitTime = serializedObject.FindProperty("waitTime");
        EditorGUILayout.PropertyField(waitTime);

        SerializedProperty slowdownDistance = serializedObject.FindProperty("slowdownDistance");
        EditorGUILayout.PropertyField(slowdownDistance);

        SerializedProperty endReachedDistance = serializedObject.FindProperty("endReachedDistance");
        EditorGUILayout.PropertyField(endReachedDistance);
    }

    private void SerializeChase() {
        SerializedProperty moveBackToOrigin = serializedObject.FindProperty("moveBackToOrigin");
        EditorGUILayout.PropertyField(moveBackToOrigin);

        SerializedProperty restingSpeed = serializedObject.FindProperty("restingSpeed");
        EditorGUILayout.PropertyField(restingSpeed);

        SerializedProperty activeSpeed = serializedObject.FindProperty("activeSpeed");
        EditorGUILayout.PropertyField(activeSpeed);

        SerializedProperty restingRadius = serializedObject.FindProperty("restingRadius");
        EditorGUILayout.PropertyField(restingRadius);
        EditorGUILayout.HelpBox("Set to 0 for unbounded radius", MessageType.None);

        SerializedProperty activeRadius = serializedObject.FindProperty("activeRadius");
        EditorGUILayout.PropertyField(activeRadius);
        EditorGUILayout.HelpBox("Set to 0 for unbounded radius", MessageType.None);

        SerializedProperty waitTime = serializedObject.FindProperty("waitTime");
        EditorGUILayout.PropertyField(waitTime);

        SerializedProperty slowdownDistance = serializedObject.FindProperty("slowdownDistance");
        EditorGUILayout.PropertyField(slowdownDistance);

        SerializedProperty endReachedDistance = serializedObject.FindProperty("endReachedDistance");
        EditorGUILayout.PropertyField(endReachedDistance);
    }

    private void SerializePatrol(bool pathFromVector) {
        if (pathFromVector) {
            SerializedProperty vectorList = serializedObject.FindProperty("vectorList");
            EditorGUILayout.PropertyField(vectorList);
        }
        else {
            SerializedProperty targetList = serializedObject.FindProperty("targetList");
            EditorGUILayout.PropertyField(targetList);
        }

        SerializedProperty restingSpeed = serializedObject.FindProperty("restingSpeed");
        EditorGUILayout.PropertyField(restingSpeed);

        SerializedProperty activeSpeed = serializedObject.FindProperty("activeSpeed");
        EditorGUILayout.PropertyField(activeSpeed);

        SerializedProperty activeRadius = serializedObject.FindProperty("activeRadius");
        EditorGUILayout.PropertyField(activeRadius);

        SerializedProperty waitTime = serializedObject.FindProperty("waitTime");
        EditorGUILayout.PropertyField(waitTime);

        SerializedProperty slowdownDistance = serializedObject.FindProperty("slowdownDistance");
        EditorGUILayout.PropertyField(slowdownDistance);

        SerializedProperty endReachedDistance = serializedObject.FindProperty("endReachedDistance");
        EditorGUILayout.PropertyField(endReachedDistance);
    }

    private void SerializeRunAway() {
        SerializedProperty moveBackToOrigin = serializedObject.FindProperty("moveBackToOrigin");
        EditorGUILayout.PropertyField(moveBackToOrigin);

        SerializedProperty restingSpeed = serializedObject.FindProperty("restingSpeed");
        EditorGUILayout.PropertyField(restingSpeed);

        SerializedProperty activeSpeed = serializedObject.FindProperty("activeSpeed");
        EditorGUILayout.PropertyField(activeSpeed);

        SerializedProperty restingRadius = serializedObject.FindProperty("restingRadius");
        EditorGUILayout.PropertyField(restingRadius);
        EditorGUILayout.HelpBox("Set to 0 for unbounded radius", MessageType.None);

        SerializedProperty activeRadius = serializedObject.FindProperty("activeRadius");
        EditorGUILayout.PropertyField(activeRadius);

        SerializedProperty waitTime = serializedObject.FindProperty("waitTime");
        EditorGUILayout.PropertyField(waitTime);

        SerializedProperty slowdownDistance = serializedObject.FindProperty("slowdownDistance");
        EditorGUILayout.PropertyField(slowdownDistance);

        SerializedProperty endReachedDistance = serializedObject.FindProperty("endReachedDistance");
        EditorGUILayout.PropertyField(endReachedDistance);
    }

    private void SerializeFollow(bool sameAsPlayerSpeed) {
        if (!sameAsPlayerSpeed) {
            SerializedProperty restingSpeed = serializedObject.FindProperty("restingSpeed");
            EditorGUILayout.PropertyField(restingSpeed);
        }

        SerializedProperty activeSpeed = serializedObject.FindProperty("activeSpeed");
        EditorGUILayout.PropertyField(activeSpeed);

        SerializedProperty activeRadius = serializedObject.FindProperty("activeRadius");
        EditorGUILayout.PropertyField(activeRadius);

        SerializedProperty separation = serializedObject.FindProperty("separation");
        EditorGUILayout.PropertyField(separation);
    }
}
