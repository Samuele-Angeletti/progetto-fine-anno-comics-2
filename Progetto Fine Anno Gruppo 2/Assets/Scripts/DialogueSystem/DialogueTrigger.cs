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

    public EDialogueInteraction modalitaDiInterazione;
    public bool CanRepeatLastDialogue;
    [ShowScriptableObject]
    public List<DialogueHolderSO> m_dialogueToShow;

   

    private void Start()
    {

        PubSub.PubSub.Subscribe(this, typeof(EndDialogueMessage));
        PubSub.PubSub.Subscribe(this, typeof(OnTriggerEnterDialogueMessage));
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
        }

    }

    public override void ShowUI(bool isVisible)
    {

    }

    public void OnPublish(IMessage message)
    {
        if (message is EndDialogueMessage)
        {
            Destroy(gameObject);

        }

        else if (message is CurrentDialogueFinishedMessage)
        {
            if (CanRepeatLastDialogue && m_dialogueToShow.Count == 1 && DialoguePlayer.Instance.standardMessageIsPlaying) return;
            else if (m_dialogueToShow.Count > 0 && !DialoguePlayer.Instance.standardMessageIsPlaying) m_dialogueToShow.RemoveAt(0);
            else if (m_dialogueToShow.Count == 0 && !DialoguePlayer.Instance.standardMessageIsPlaying)PubSub.PubSub.Publish(new EndDialogueMessage());

        }
        else if (message is OnTriggerEnterDialogueMessage)
        {
            PubSub.PubSub.Publish(new StartDialogueMessage(m_dialogueToShow[0].Dialogo));
        }
    }

    

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(EndDialogueMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(OnTriggerEnterDialogueMessage));
        PubSub.PubSub.Unsubscribe(this, typeof (CurrentDialogueFinishedMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(OnInteractionDialogueMessage));


    }
}

