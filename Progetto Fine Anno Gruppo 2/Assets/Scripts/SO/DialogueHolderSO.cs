using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueSO")]
public class DialogueHolderSO : ScriptableObject
{
    public List<DialogueLine> Dialogo;
}


[System.Serializable]
public struct DialogueLine
{

    public ESpeaker WhoIsSpeaking;
    [TextArea(0,10)]
    public string DialougueString;




}