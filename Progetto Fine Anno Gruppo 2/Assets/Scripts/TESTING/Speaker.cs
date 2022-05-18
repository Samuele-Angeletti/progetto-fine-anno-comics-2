using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    DialogueHolderSO dialogoDaPrendere;
    DialogueTrigger trigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigger = collision.GetComponent<DialogueTrigger>();
        dialogoDaPrendere = collision.GetComponent<DialogueTrigger>().m_dialogueToShow;
        if (dialogoDaPrendere == null && trigger == null && trigger.modalit‡DiInterazione == EInteraction.OnTriggerEnter)
            PubSub.PubSub.Publish(new StartDialogueMessage(dialogoDaPrendere));
        else
            return;
        
      
        
        
    }

   
}
