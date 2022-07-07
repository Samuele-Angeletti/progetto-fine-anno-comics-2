using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class StartDialogueMessage : IMessage
{
    public List<DialogueLine> dialogue;
    public DialogueHolderSO dialogueHolder;
    public bool canRepeatLastDialogue;
    public StartDialogueMessage(List<DialogueLine> dialogue,bool canRepeatLastDialogue)
    {
        this.dialogue = dialogue;
        this.canRepeatLastDialogue = canRepeatLastDialogue;

    }

    
}
