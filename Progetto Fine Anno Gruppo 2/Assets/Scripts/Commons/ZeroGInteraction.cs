using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
public class ZeroGInteraction : MonoBehaviour, IInteractable, ISubscriber
{
    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(ZeroGMessage));
    }
    private bool m_Interacted;
    public void Interact()
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

    public void ShowUI(bool isVisible)
    {

    }

    public void OnPublish(IMessage message)
    {
        if(message is ZeroGMessage)
        {
            ZeroGMessage zeroGMessage = (ZeroGMessage)message;
            m_Interacted = zeroGMessage.Active;
        }
    }

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(ZeroGMessage));
    }

    private void OnDestroy()
    {
        OnDisableSubscribe();
    }
}
