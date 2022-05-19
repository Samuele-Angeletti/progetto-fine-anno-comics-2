using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;

public class ButtonInteraction : Interactable, ISubscriber
{

    public override void Interact(Interacter interacter)
    {
        switch (InteractionType)
        {
            case EInteractionType.ZeroG:
                ZeroGInteraction();
                break;
            case EInteractionType.GoToCheckPoint:
                interacter.transform.position = InterestedObject.transform.position;
                break;
            case EInteractionType.Action:
                InterestedObject.GetComponentInChildren<Interactable>().Interact(interacter);
                break;
        }
    }

    public override void ShowUI(bool isVisible)
    {

    }

    private bool m_Interacted;


    private void Start()
    {
        if (InteractionType == EInteractionType.ZeroG)
        {
            PubSub.PubSub.Subscribe(this, typeof(ZeroGMessage));
        }
    }


    private void ZeroGInteraction()
    {
        if (!m_Interacted)
        {
            PubSub.PubSub.Publish(new ZeroGMessage(true));
            m_Interacted = true;
        }
        else
        {
            PubSub.PubSub.Publish(new ZeroGMessage(false));
            m_Interacted = false;
        }
    }


    public void OnPublish(IMessage message)
    {

        if (InteractionType == EInteractionType.ZeroG)
        {
            if (message is ZeroGMessage)
            {
                ZeroGMessage zeroGMessage = (ZeroGMessage)message;
                m_Interacted = zeroGMessage.Active;
            }
        }
    }

    public void OnDisableSubscribe()
    {

        if (InteractionType == EInteractionType.ZeroG)
            PubSub.PubSub.Unsubscribe(this, typeof(ZeroGMessage));
    }

    private void OnDestroy()
    {
        OnDisableSubscribe();
    }
}
