using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class StartDialogueMessage : IMessage
{
    public DialogueHolderSO dialogue;
    public DialogueTrigger trigger;
    public StartDialogueMessage(DialogueHolderSO dialogue)
    {
        this.dialogue = dialogue;

    }
}
