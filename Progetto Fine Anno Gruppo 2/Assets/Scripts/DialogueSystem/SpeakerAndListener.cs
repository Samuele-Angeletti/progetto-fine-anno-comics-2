using Commons;
using MainGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerAndListener : MonoBehaviour
{
    DialogueTrigger m_dialogueTrigger;
    AudioTrigger m_audioTrigger;
    EventArea m_eventArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_dialogueTrigger = collision.GetComponent<DialogueTrigger>();
        m_audioTrigger = collision.GetComponent<AudioTrigger>();
        if (m_dialogueTrigger != null && m_dialogueTrigger.modalitaDiInterazione == EDialogueInteraction.OnTriggerEnter && m_dialogueTrigger.m_dialogueToShow.Count >0)
        {
            m_dialogueTrigger.enabled = true;
            PubSub.PubSub.Publish(new StartDialogueMessage(m_dialogueTrigger.m_dialogueToShow[0].Dialogo,false));
        }
        if (m_audioTrigger != null)
        {
            PubSub.PubSub.Publish(new OnTriggerEnterAudio());
        }

       
            
       
      
        
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        m_dialogueTrigger = collision.GetComponent<DialogueTrigger>();
        m_audioTrigger = collision.GetComponent<AudioTrigger>();
        if (m_dialogueTrigger != null && m_dialogueTrigger.modalitaDiInterazione == EDialogueInteraction.OnTriggerEnter)
        {
            m_dialogueTrigger.enabled =false;
        }
        if (m_audioTrigger != null)
        {
            PubSub.PubSub.Publish(new OnTriggerExitAudio());
        }

      
    }


}
