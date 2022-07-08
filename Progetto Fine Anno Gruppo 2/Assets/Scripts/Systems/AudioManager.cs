using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using PubSub;
using System.Collections.Generic;
using System.Collections;

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour, ISubscriber
{
    #region SINGLETONE
    private static AudioManager m_instance;
    public static AudioManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<AudioManager>();
                if (m_instance == null)
                {
                    GameObject temp = new GameObject("Audio Manager");
                    m_instance = temp.AddComponent<AudioManager>();
                    DontDestroyOnLoad(temp);
                }
            }
            return m_instance;
        }
    }
   
    private void Awake()
    {
        Initialize();

    }

    private void Initialize()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        m_AudioSource.outputAudioMixerGroup = m_audioMixer.FindMatchingGroups("BackGroundMusic").Single();
        

    }


    #endregion

    [Header("MENU AUDIO")]
    [SerializeField] AudioClip m_musicaMenùPrincipale;
    [Header("AUDIO REFERENCE")]
    [SerializeField] AudioSource m_AudioSource;
    [SerializeField] AudioMixer m_audioMixer;
    [Header("FADING SETTINGS")]
    public float fadeInTime;
    public float fadeOutTime;
    
    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(SendAudioSettingsMessage));
        PubSub.PubSub.Subscribe(this, typeof(OnTriggerEnterAudio));
        PubSub.PubSub.Subscribe(this, typeof(SendAudioMessage));
        PubSub.PubSub.Publish(new SendAudioMessage(m_musicaMenùPrincipale));

        PubSub.PubSub.Publish(new SendAudioSettingsMessage(true,true));
        if (m_musicaMenùPrincipale == null) return;
        
    }


    public void ChangeVolumeSFX(float value)
    {
        m_audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
    }
    public void ChangeVolumeMusic(float value)
    {
        m_audioMixer.SetFloat("BackGroundMusic", Mathf.Log10(value) * 20);

    }
    public void ChangeVolumeAmbience(float value)
    {
        m_audioMixer.SetFloat("AmbientSound", Mathf.Log10(value) * 20);

    }
    public void ChangeMasterVolume(float value)
    {
        m_audioMixer.SetFloat("Master_Volume", Mathf.Log10(value) * 20);
    }
   
    

    public void OnPublish(IMessage message)
    {
        if (message is SendAudioMessage)
        {
            SendAudioMessage audio = (SendAudioMessage)message;
            if (m_AudioSource.clip != null)
            {
                PubSub.PubSub.Publish(new GetAudioBeforeChangingMessage(m_AudioSource.clip));
                StartCoroutine(WaitForFade(m_AudioSource, fadeOutTime, 0));
                m_AudioSource.clip = audio.audioClipToSend;
                StartCoroutine(WaitForFade(m_AudioSource, fadeInTime, 1));

            }
            else if (m_AudioSource.clip == null)
            {
                m_AudioSource.clip = audio.audioClipToSend;
                StartCoroutine(WaitForFade(m_AudioSource, fadeInTime, 1));

            }

        }
        else if (message is SendAudioSettingsMessage)
        {
            SendAudioSettingsMessage settings = (SendAudioSettingsMessage)message;
            m_AudioSource.playOnAwake = settings.m_PlayOnAwake;
            m_AudioSource.loop = settings.m_CanLoop;
        }
    }

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Subscribe(this, typeof(OnTriggerEnterAudio));
        PubSub.PubSub.Subscribe(this, typeof(SendAudioMessage));
        PubSub.PubSub.Subscribe(this, typeof(SendAudioSettingsMessage));
    }


    public IEnumerator WaitForFade(AudioSource audioSource, float duration, float targetVolume)
    {
        var courutine = this.RunCoroutine(StartFade(audioSource, duration, targetVolume));
        yield return new WaitWhile(() => m_AudioSource.volume != targetVolume);
    }


    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }



}
