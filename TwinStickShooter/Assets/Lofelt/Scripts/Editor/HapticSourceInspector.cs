using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(HapticSource))]
[CanEditMultipleObjects]
/// <summary>
/// Provides an inspector for the <c>HapticSource</c> component.
/// </summary>
///
/// The inspector lets you link a <c>HapticSource</c> to a <c>HapticClip</c>.
public class HapticSourceInspector : Editor
{
    string hapticsDirectory;

    SerializedProperty hapticClip;

    public static GUIContent hapticClipLabel = EditorGUIUtility.TrTextContent("Haptic Clip", "The HapticClip asset played by the HapticSource.");
    public static GUIContent priorityLabel = EditorGUIUtility.TrTextContent("Priority", "Sets the priority of the source.Note that a haptics with a larger priority value will be stolen by haptics with smaller priority values.");

    void OnEnable()
    {
        hapticClip = serializedObject.FindProperty("clip");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        HapticSource hapticSource = (HapticSource)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(hapticClip, hapticClipLabel);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        CreatePrioritySlider(hapticSource);

        serializedObject.ApplyModifiedProperties();
    }

    /// Helper function to create a priority slider for haptic source with High and Max text labels.
    void CreatePrioritySlider(HapticSource hapticSource)
    {
        Rect position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);

        // Draw label
        position = EditorGUI.PrefixLabel(position, priorityLabel);
        EditorGUI.indentLevel--;

        hapticSource.priority = EditorGUI.IntSlider(position, hapticSource.priority, 0, 256);
        float labelWidth = position.width;

        // Move to next line
        position.y += EditorGUIUtility.singleLineHeight;

        // Subtract the text field width thats drawn with slider
        position.width -= EditorGUIUtility.fieldWidth;

        GUIStyle style = GUI.skin.label;
        TextAnchor defaultAlignment = GUI.skin.label.alignment;
        style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "High", style);
        style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "Low", style);
        GUI.skin.label.alignment = defaultAlignment;

        // Allow space for the High/Low labels
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
}
