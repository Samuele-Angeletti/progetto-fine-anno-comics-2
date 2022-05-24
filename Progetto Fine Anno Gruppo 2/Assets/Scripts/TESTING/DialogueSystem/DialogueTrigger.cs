using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public enum EDialogueInteraction
{
    OnTriggerEnter,
    OnInteraction
}
[RequireComponent(typeof(BoxCollider2D))]
public class DialogueTrigger : Interactable, ISubscriber
{
    private bool m_Interacted;
    public EDialogueInteraction modalitaDiInterazione;
    [ShowScriptableObject]
    public DialogueHolderSO m_dialogueToShow;
    

    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(EndDialogueMessage));
    }
    public override void Interact(Interacter interacter)
    {
        m_Interacted = true;
      
    }
    private void Update()
    {
        if (m_Interacted && DialogueManager.Instance.dialogueIsPlaying == false)
        {
            if (modalitaDiInterazione == EDialogueInteraction.OnTriggerEnter)
            {
                return;
            }
            PubSub.PubSub.Publish(new StartDialogueMessage(m_dialogueToShow));
            m_Interacted = false;

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
    }

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(EndDialogueMessage));

    }
}

