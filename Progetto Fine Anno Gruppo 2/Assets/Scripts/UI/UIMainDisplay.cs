using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using TMPro;

public class UIMainDisplay : MonoBehaviour, ISubscriber
{
    Animator animator;

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(StartEngineModuleMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));

    }

    public void OnPublish(IMessage message)
    {
        if(message is StartEngineModuleMessage)
        {
            animator.SetBool("ActiveModule", true);
        }
        else if(message is DockingCompleteMessage)
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
    }

}