using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
#if UNITY_EDITOR
using UnityEngine;

[CustomEditor(typeof(DialogueTrigger))]
public class DialogueTriggerEditor : Editor
{
    private Editor m_editor;
    public SerializedProperty m_scriptableObject;
    private SerializedProperty m_isFinished;
    private void OnEnable()
    {
    }
    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        m_scriptableObject = serializedObject.FindProperty("m_dialogueToShow");
        m_isFinished = serializedObject.FindProperty("isFinished");

        EditorGUILayout.PropertyField(m_scriptableObject, new GUIContent("Dialogue To Show"), GUILayout.Height(20));
        serializedObject.ApplyModifiedProperties();
        if (m_scriptableObject.objectReferenceValue == null)
        {
            if (GUILayout.Button("Crea Nuovo Dialogo"))
            {
                DialogueHolderSO dialogueInstantieted =(DialogueHolderSO)ScriptableObject.CreateInstance(typeof(DialogueHolderSO));
                m_scriptableObject.objectReferenceValue = dialogueInstantieted;
                serializedObject.ApplyModifiedProperties();
            }
            return;

        }
        EditorGUILayout.PropertyField(m_isFinished, new GUIContent("isFinished"), GUILayout.Height(20));
        CreateCachedEditor(m_scriptableObject.objectReferenceValue, null, ref m_editor);
        m_editor.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();


    }
   
    
}
#endif
