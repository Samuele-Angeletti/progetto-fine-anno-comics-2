using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
public class UIExternal : MonoBehaviour, ISubscriber
{
    Animator animator;

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(StartEngineModuleMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));

    }

    public void OnPublish(IMessage message)
    {
        if (message is StartEngineModuleMessage)
        {
            animator.SetBool("ActiveModule", true);
        }
        else if (message is DockingCompleteMessage || message is NoBatteryMessage || message is ModuleDestroyedMessage)
        {
            animator.SetBool("ActiveModule", false);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));
        PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
        PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
        PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
    }
}
