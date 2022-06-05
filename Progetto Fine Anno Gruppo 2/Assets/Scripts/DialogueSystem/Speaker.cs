using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    DialogueTrigger trigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigger = collision.GetComponent<DialogueTrigger>();
        if (trigger != null && trigger.modalitaDiInterazione == EDialogueInteraction.OnTriggerEnter)
        {
            PubSub.PubSub.Publish(new OnTriggerEnterMessage());
        }
        else
            return;
        
      
        
        
    }

   
}
