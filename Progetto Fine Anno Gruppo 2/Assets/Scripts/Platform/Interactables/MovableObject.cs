using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
public class MovableObject : Interactable, ISubscriber
{
    [Header("Movable Object Settings")]
    [SerializeField] Transform m_EndPivot;
    [SerializeField] float m_TransitionSpeed;
    [SerializeField] bool m_OneShot;
    [SerializeField] bool m_RevertOnTriggerExit;
    private Vector3 m_StartPivot;
    private bool m_Active;
    private bool m_Used;

    private void Start()
    {

        PubSub.PubSub.Subscribe(this, typeof(ActivableMessage));
    }
    public override void Interact(Interacter interacter)
    {
        Active();
    }

    private void Update()
    {
        if(m_Active)
        {
            transform.position = Vector3.Lerp(transform.position, m_EndPivot.position, m_TransitionSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, m_EndPivot.position) < 0.1f)
            {
                m_Active = false;
                transform.position = m_EndPivot.position;
                m_EndPivot.position = m_StartPivot;
            }
        }
    }

    public override void ShowUI(bool isVisible)
    {
    }
    public void OnPublish(IMessage message)
    {
        if (message is ActivableMessage)
        {
            ActivableMessage activableMessage = (ActivableMessage)message;
            if (activableMessage.Activator.gameObject == InterestedObject && activableMessage.Active)
            {
                if(!m_Active) Active();
            }
            else if(activableMessage.Activator.gameObject == InterestedObject && !activableMessage.Active && m_RevertOnTriggerExit)
            {
                if(!m_Active) Active();
            }
        }
    }

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(ActivableMessage));
    }

    private void Active()
    {
        if(m_OneShot && !m_Used)
        {
            m_Used = true;
            m_Active = true;
            m_StartPivot = transform.position;
        }
        else if(!m_OneShot)
        {
            m_Active = true;
            m_StartPivot = transform.position;
        }
    }
}
