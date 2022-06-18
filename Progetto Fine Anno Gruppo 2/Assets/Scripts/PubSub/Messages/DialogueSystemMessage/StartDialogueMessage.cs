using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class StartDialogueMessage : IMessage
{
    public List<DialogueLine> dialogue;
    public StartDialogueMessage(List<DialogueLine> dialogue)
    {
        this.dialogue = dialogue;

    }

    
}
