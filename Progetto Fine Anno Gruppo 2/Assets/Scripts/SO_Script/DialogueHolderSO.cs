using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="DialogueSO")]
public class DialogueHolderSO : ScriptableObject
{
    [Header("Settings Dialogo")]
    public bool IsSkippable;
    public List<DialogueLineSO> Dialogo;
}


[System.Serializable]
public struct DialogueLineSO
{
    public ESpeaker WhoIsSpeaking;
    [TextArea]
    public string DialougueLine;
    public bool IsTheLastOne;



}