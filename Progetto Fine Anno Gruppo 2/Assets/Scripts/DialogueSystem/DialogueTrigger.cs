using Commons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PubSub;
using System;

public enum EDialogueInteraction
{
    OnTriggerEnter,
    OnInteraction
}
[RequireComponent(typeof(BoxCollider2D))]
public class DialogueTrigger : Interactable, ISubscriber
{
    private bool m_Interacted;
    DialogueHolderSO lastDialogue;
    public EDialogueInteraction modalitaDiInterazione;
    public bool CanRepeatLastDialogue = true;
    [ShowScriptableObject]
    public List<DialogueHolderSO> m_dialogueToShow;

   

    private void Start()
    {
        lastDialogue = m_dialogueToShow.Last();
        PubSub.PubSub.Subscribe(this, typeof(EndDialogueMessage));
        PubSub.PubSub.Subscribe(this, typeof(OnTriggerEnterMessage));
        PubSub.PubSub.Subscribe(this, typeof(CurrentDialogueFinishedMessage));
        PubSub.PubSub.Subscribe(this, typeof(OnInteractionDialogueMessage));
    }
    
    public override void Interact(Interacter interacter)
    {
        if (interacter.InteractionAvailable)
        {

            if (m_dialogueToShow.Count >= 1)
            {
                PubSub.PubSub.Publish(new StartDialogueMessage(m_dialogueToShow[0].Dialogo));
            }

            else if (CanRepeatLastDialogue && m_dialogueToShow.Count == 1)
            {
                PubSub.PubSub.Publish(new StartDialogueMessage(lastDialogue.Dialogo));

            }


        }

    }
    private void Update()
    {
      

    }
    public override void ShowUI(bool isVisible)
    {

    }

    public void OnPublish(IMessage message)
    {
        if (message is EndDialogueMessage)
        {
            if (CanRepeatLastDialogue)
            {
                return;
            }
            else
                Destroy(gameObject);
        }

        else if (message is CurrentDialogueFinishedMessage)
        {

            if (m_dialogueToShow.Count > 1)
            {
                m_Interacted = false;
                m_dialogueToShow.RemoveAt(0);

            }
            
          
            else if (m_dialogueToShow.Count == 0)
            {
                PubSub.PubSub.Publish(new EndDialogueMessage());
            }

        }
        else if (message is OnTriggerEnterMessage)
        {
            PubSub.PubSub.Publish(new StartDialogueMessage(m_dialogueToShow[0].Dialogo));
        }
        else if (message is OnInteractionDialogueMessage)
        {
            
        }
    }

    

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(EndDialogueMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(OnTriggerEnterMessage));
        PubSub.PubSub.Unsubscribe(this, typeof (CurrentDialogueFinishedMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(OnInteractionDialogueMessage));


    }
}

