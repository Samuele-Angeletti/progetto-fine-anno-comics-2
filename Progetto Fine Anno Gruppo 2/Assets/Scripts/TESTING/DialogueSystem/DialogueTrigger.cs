using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInteraction
{
    OnTriggerEnter,
    OnInteraction
}
[RequireComponent(typeof(BoxCollider2D))]
public class DialogueTrigger : Interactable
{
    public bool m_CanInteract = false;
    public EInteraction modalit‡DiInterazione;
    [ShowScriptableObject]
    public DialogueHolderSO m_dialogueToShow;

    public override void Interact(Interacter interacter)
    {
        if (m_CanInteract == false) return;
        PubSub.PubSub.Publish(new StartDialogueMessage(m_dialogueToShow));
    }

    public override void ShowUI(bool isVisible)
    {
    }

    private void OnValidate()
    {
        switch (modalit‡DiInterazione)
        {
            case EInteraction.OnTriggerEnter:
                break;
            case EInteraction.OnInteraction:
                m_CanInteract = true;
                break;
            

        }
            
    }

}

