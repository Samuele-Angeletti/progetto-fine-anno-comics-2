using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialoguePlayer))]
public class DIalogueManagerEditor : Editor
{
    private SerializedProperty adaProperty;
    private SerializedProperty playerProperty;

    public static readonly GUIContent adaSprite = EditorGUIUtility.TrTextContent("Ada Sprite");
    public static readonly GUIContent playerSprite = EditorGUIUtility.TrTextContent("Player Sprite");

   
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        adaProperty = serializedObject.FindProperty("spriteAda");
        playerProperty = serializedObject.FindProperty("spritePlayer");

        adaProperty.objectReferenceValue = EditorGUILayout.ObjectField(adaSprite, adaProperty.objectReferenceValue, typeof(Sprite), false, GUILayout.Height(75))as Sprite;
        playerProperty.objectReferenceValue = EditorGUILayout.ObjectField(playerSprite, playerProperty.objectReferenceValue, typeof(Sprite), false, GUILayout.Height(75)) as Sprite;


        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();

        }

    }
    
}
