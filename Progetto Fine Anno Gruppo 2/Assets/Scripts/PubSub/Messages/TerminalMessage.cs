using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
public class TerminalMessage : IMessage
{
    public TerminalScriptableObject TerminalScriptableObject;

    public TerminalMessage(TerminalScriptableObject terminalScriptableObject)
    {
        TerminalScriptableObject = terminalScriptableObject;
    }
}
