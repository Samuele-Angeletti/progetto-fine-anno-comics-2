using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueTrigger))]
public class DialogueTriggerEditor : Editor
{
    private Editor m_editor;
    public SerializedProperty m_scriptableObject;
    private void OnEnable()
    {
    }
    public override void OnInspectorGUI()
    {
        

        serializedObject.Update();
        m_scriptableObject = serializedObject.FindProperty("m_dialogueToShow");
        EditorGUILayout.PropertyField(m_scriptableObject,new GUIContent("Dialogue To Show"), GUILayout.Height(20));
        serializedObject.ApplyModifiedProperties();

        if (m_scriptableObject.objectReferenceValue == null)
        {
            return;
        }
        CreateCachedEditor(m_scriptableObject.objectReferenceValue, null, ref m_editor);
        m_editor.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();


    }
}
