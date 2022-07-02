using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerAndListener : MonoBehaviour
{
    DialogueTrigger m_dialoguetrigger;
    AudioTrigger m_audioTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_dialoguetrigger = collision.GetComponent<DialogueTrigger>();
        m_audioTrigger = collision.GetComponent<AudioTrigger>();

        if (m_dialoguetrigger != null  && m_dialoguetrigger.modalitaDiInterazione == EDialogueInteraction.OnTriggerEnter)
        {
            PubSub.PubSub.Publish(new OnTriggerEnterDialogueMessage());
        }
        else if (m_audioTrigger != null)
        {
            PubSub.PubSub.Publish(new OnTriggerEnterAudio());
        }
            return;
       
      
        
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        m_audioTrigger = collision.GetComponent<AudioTrigger>();
        if (m_audioTrigger != null)
        {
            PubSub.PubSub.Publish(new OnTriggerExitAudio());
        }
    }


}
