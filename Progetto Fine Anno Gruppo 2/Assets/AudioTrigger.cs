using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using UnityEngine.Audio;

[RequireComponent(typeof(Collider2D))]
public class AudioTrigger : MonoBehaviour, ISubscriber
{
    [Header("AUDIO SETTINGS")]
    [SerializeField] bool m_canLoop;
    [SerializeField] bool m_canPlayOnAwake;
    private Collider2D m_collider;
    [Header("AUDIO SO")]
    [ShowScriptableObject]
    public AudioHolder musicToSend;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        
    }
    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(OnTriggerEnterAudio));
        PubSub.PubSub.Subscribe(this, typeof(SendAudioSettingsMessage));
    }

   
    public void OnPublish(IMessage message)
    {
        if (message is OnTriggerEnterAudio)
        {
            PubSub.PubSub.Publish(new SendAudioMessage(musicToSend));
        }
    }
    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(OnTriggerEnterAudio));
    }
    
}
