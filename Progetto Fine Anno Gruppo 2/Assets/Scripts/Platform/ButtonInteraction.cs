using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;
using MainGame;
public class ButtonInteraction : Interactable,ISubscriber
{
    private AudioSource m_soundSource;
    [Header("Button Interaction Settings")]
    [SerializeField] bool m_Active = true;
    [SerializeField] bool m_OneShot;
    [SerializeField] AudioHolder m_audioHolder;
    private void Awake()
    {
        m_soundSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (InteractionType == EInteractionType.PlayPacMan || InteractionType == EInteractionType.ActiveModule)
        {
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
        }
    }
    public override void Interact(Interacter interacter)
    {
        if (m_Active)
        {
            if(m_soundSource != null) m_soundSource.PlayOneShot(m_audioHolder.audioToSend.musicToPlay);
            GameManager.Instance.GetButtonInteractionSO(InteractionType).Interact(InterestedObject, interacter);
            if (m_OneShot) m_Active = false;
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
    }

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));

    }

    private void OnDestroy()
    {
        OnDisableSubscribe();
    }
}
