using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    DialogueHolderSO dialogoDaPrendere;
    DialogueTrigger trigger;
    public ESpeaker soggetto;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigger = collision.GetComponent<DialogueTrigger>();
        dialogoDaPrendere = collision.GetComponent<DialogueTrigger>().m_dialogueToShow;
        if (dialogoDaPrendere == null && trigger == null) return;
        PubSub.PubSub.Publish(new StartDialogueMessage(dialogoDaPrendere));
        
    }

   
}
