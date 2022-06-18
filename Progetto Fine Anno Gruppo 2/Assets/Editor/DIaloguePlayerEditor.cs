using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialoguePlayer))]
public class DIaloguePlayerEditor : Editor
{
    private SerializedProperty playerProperty;
    private SerializedProperty adaPropertyPreIntegrazione;
    private SerializedProperty adaPropertyPrimaIntegrazione;
    private SerializedProperty adaPropertySecondaIntegrazione;
    private SerializedProperty adaPropertyFormaFinale;

    public static readonly GUIContent GUIContentPlayerSprite = EditorGUIUtility.TrTextContent("Player Sprite");
    public static readonly GUIContent GUIContentSpriteAdaPreIntegrazione = EditorGUIUtility.TrTextContent("Sprite Ada Pre Integrazione");
    public static readonly GUIContent GUIContentSpriteAdaPrimaIntegrazione = EditorGUIUtility.TrTextContent("Sprite Ada Prima Integrazione");
    public static readonly GUIContent GUIContentSpriteAdaSecondaIntegrazione = EditorGUIUtility.TrTextContent("Sprite Ada Seconda Integrazione");
    public static readonly GUIContent GUIContentSpriteAdaFormaFinale = EditorGUIUtility.TrTextContent("Sprite Ada Forma Finale");

   
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        serializedObject.Update();
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUI.BeginChangeCheck();

            playerProperty = serializedObject.FindProperty("spritePlayer");
            adaPropertyPreIntegrazione = serializedObject.FindProperty("spriteAdaPreIntegrazione");
            adaPropertyPrimaIntegrazione = serializedObject.FindProperty("spriteAdaPrimaIntegrazione");
            adaPropertySecondaIntegrazione = serializedObject.FindProperty("spriteAdaSecondaIntegrazione");
            adaPropertyFormaFinale = serializedObject.FindProperty("spriteAdaFormaFinale");


            playerProperty.objectReferenceValue = EditorGUILayout.ObjectField(GUIContentPlayerSprite, playerProperty.objectReferenceValue, typeof(Sprite), false, GUILayout.Height(75)) as Sprite;
            adaPropertyPreIntegrazione.objectReferenceValue = EditorGUILayout.ObjectField(GUIContentSpriteAdaPreIntegrazione, adaPropertyPreIntegrazione.objectReferenceValue, typeof(Sprite), false, GUILayout.Height(75)) as Sprite;
            adaPropertyPrimaIntegrazione.objectReferenceValue = EditorGUILayout.ObjectField(GUIContentSpriteAdaPrimaIntegrazione, adaPropertyPrimaIntegrazione.objectReferenceValue, typeof(Sprite), false, GUILayout.Height(75)) as Sprite;
            adaPropertySecondaIntegrazione.objectReferenceValue = EditorGUILayout.ObjectField(GUIContentSpriteAdaSecondaIntegrazione, adaPropertySecondaIntegrazione.objectReferenceValue, typeof(Sprite), false, GUILayout.Height(75)) as Sprite;
            adaPropertyFormaFinale.objectReferenceValue = EditorGUILayout.ObjectField(GUIContentSpriteAdaFormaFinale, adaPropertyFormaFinale.objectReferenceValue, typeof(Sprite), false, GUILayout.Height(75)) as Sprite;


            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();

            }
        }
        

    }
    
}
