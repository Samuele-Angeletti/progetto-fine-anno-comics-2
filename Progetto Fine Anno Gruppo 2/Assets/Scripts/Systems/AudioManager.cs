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
    [SerializeField] AudioHolder m_menuAudioHolder;
    [Header("AUDIO REFERENCE")]
    [SerializeField] AudioSource m_AudioSource;
    [SerializeField] AudioMixer m_audioMixer;
    [Header("FADING SETTINGS")]
    [SerializeField] float fadeInTime;
    [SerializeField] float fadeOutTime;


    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(OnTriggerEnterAudio));
        PubSub.PubSub.Subscribe(this, typeof(SendAudioMessage));
        PubSub.PubSub.Subscribe(this, typeof(SendAudioSettingsMessage));

        PubSub.PubSub.Publish(new SendAudioMessage(m_menuAudioHolder));
        PubSub.PubSub.Publish(new SendAudioSettingsMessage(true,true));
    }


    public void ChangeVolumeSFX(float value)
    {
        m_audioMixer.SetFloat("SFX", value);
    }
    public void ChangeVolumeMusic(float value)
    {
        m_audioMixer.SetFloat("BackGroundMusic", value);

    }
    public void ChangeVolumeAmbience(float value)
    {
        m_audioMixer.SetFloat("AmbientSound", value);

    }
    public void ChangeMasterVolume(float value)
    {
        m_audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }
   
    

    public void OnPublish(IMessage message)
    {
        if (message is SendAudioMessage)
        {
            SendAudioMessage audio = (SendAudioMessage)message;
            if (m_AudioSource.clip != null)
            {
                StartCoroutine(FadeOut(m_AudioSource, fadeOutTime));
                m_AudioSource.clip = audio.audioHolderToSend.audioToSend.musicToPlay;
                StartCoroutine(FadeIn(m_AudioSource, m_audioMixer, fadeInTime));

            }
            else if (m_AudioSource.clip == null)
            {
                m_AudioSource.clip = audio.audioHolderToSend.audioToSend.musicToPlay;
                StartCoroutine(FadeIn(m_AudioSource, m_audioMixer, fadeInTime));

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
    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeIn(AudioSource audioSource,AudioMixer mixer, float FadeTime)
    {
        float startVolume = 0.2f;
       

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }
}
