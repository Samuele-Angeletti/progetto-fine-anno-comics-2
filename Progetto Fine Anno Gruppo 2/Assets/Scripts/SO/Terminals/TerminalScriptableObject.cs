using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerminalSO", menuName = "SO/Terminal")]
public class TerminalScriptableObject : ScriptableObject
{
    public string Title;
    public string ProgressiveNumber;
    [TextArea(0,10)]
    public string Content;
    public string Author;

    public string UniqueID { get; private set; } = "";
    
    public void GenerateID()
    {
        UniqueID = Title + ProgressiveNumber + Random.Range(0.0001f, 35.4f);
    }
}
