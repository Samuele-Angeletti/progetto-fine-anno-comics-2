using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;
using MainGame;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class ButtonInteraction : Interactable, ISubscriber
{
    private AudioSource m_soundSource;
    
    [Header("Button Interaction Settings")]
    [SerializeField] bool m_Active = true;
    [SerializeField] bool m_OneShot;
    [SerializeField] Light2D m_Activationlight;
    [SerializeField] UnityEvent m_SceneEvent;
    [Header("Audio Interaction")]
    [SerializeField] AudioClip m_InteractioneAudio;



    BoxCollider2D m_TriggerCollider;

    private void Awake()
    {
        m_soundSource = GetComponent<AudioSource>();
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].isTrigger)
            {
                m_TriggerCollider = colliders[i];
            }
        }

    }

    private void Start()
    {
        if (InteractionType == EInteractionType.PlayPacMan || InteractionType == EInteractionType.ActiveModule)
        {
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
        }

        PubSub.PubSub.Subscribe(this, typeof(ZeroGMessage));
    }
    public override void Interact(Interacter interacter)
    {
        if (m_Active)
        {

            if (m_soundSource != null && m_InteractioneAudio != null) m_soundSource.PlayOneShot(m_InteractioneAudio);
            GameManager.Instance.GetButtonInteractionSO(InteractionType).Interact(InterestedObject, interacter, this);
            
            if (m_OneShot) m_Active = false;

            m_SceneEvent.Invoke();
        }
    }

    public override void ShowUI(bool isVisible)
    {

    }

    public void SetActive(bool active)
    {
        m_Active = active;
    }

    public void SetInterestedObject(GameObject newInterestedObject)
    {
        InterestedObject = newInterestedObject;
    }

    public void OnPublish(IMessage message)
    {
        if (message is NoBatteryMessage || message is ModuleDestroyedMessage)
        {
            m_Active = true;
        }
        else if(message is ZeroGMessage)
        {
            m_Active = true;
            Invoke("Unsubscribe", 1f);
        }
    }

    private void Unsubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(ZeroGMessage));
    }

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(ZeroGMessage));

    }

    private void OnDestroy()
    {
        OnDisableSubscribe();
    }

    public void ActiveTrigger(bool active)
    {
        m_TriggerCollider.enabled = active;
    }

    public void ActiveLight()
    {
        if(m_Activationlight != null)
            m_Activationlight.enabled = !m_Activationlight.enabled;
    }
}
