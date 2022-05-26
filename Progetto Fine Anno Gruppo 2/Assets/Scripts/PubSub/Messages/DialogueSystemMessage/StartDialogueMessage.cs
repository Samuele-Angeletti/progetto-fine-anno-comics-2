using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class StartDialogueMessage : IMessage
{
    public DialogueHolderSO dialogue;
    public StartDialogueMessage(DialogueHolderSO dialogue)
    {
        this.dialogue = dialogue;

    }
}
