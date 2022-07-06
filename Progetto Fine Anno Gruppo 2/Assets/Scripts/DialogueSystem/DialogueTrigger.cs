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

        PubSub.PubSub.Subscribe(this, typeof(CurrentDialogueFinishedMessage));
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
        if (message is CurrentDialogueFinishedMessage)
        {
            if (CanRepeatLastDialogue && m_dialogueToShow.Count == 1 && !DialoguePlayer.Instance.standardMessageIsPlaying) return;
            if (m_dialogueToShow.Count > 0 && !DialoguePlayer.Instance.standardMessageIsPlaying) m_dialogueToShow.RemoveAt(0);
            if (m_dialogueToShow.Count == 0 && !DialoguePlayer.Instance.standardMessageIsPlaying) return;

        }
    }

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof (CurrentDialogueFinishedMessage));


    }

}
