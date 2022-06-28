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
        m_firstAudioSource.outputAudioMixerGroup = m_audioMixer.FindMatchingGroups("BackGroundMusic").Single();
        m_secondAudioSource.outputAudioMixerGroup = m_audioMixer.FindMatchingGroups("BackGroundMusic").Single();

    }





    #endregion
    private AudioSource m_currentAudioSource;
    private AudioClip m_currentPlayingMusic;
    private int audioIndex;
    private Dictionary<int, AudioSource> m_audioSourcesWithIndex;

    [SerializeField] AudioSource m_firstAudioSource;
    [SerializeField] AudioSource m_secondAudioSource;

    [SerializeField] AudioMixer m_audioMixer;
    [SerializeField] float fadinTime;
    [SerializeField] float targetVolume;

    

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
        m_audioMixer.SetFloat("Master", value);
    }
   
    

    public void OnPublish(IMessage message)
    {
        if (message is SendAudioMessage)
        {
            AudioHolder audio = (AudioHolder)message;
            if (m_currentPlayingMusic != null)
            {
                StartCoroutine(StartFade(m_audioMixer, "BackGroundMusic", fadinTime, targetVolume));
                m_currentPlayingMusic = audio.audioToSend.musicToPlay;
                m_currentAudioSource.Play();
            }

        }
        else if (message is SendAudioSettingsMessage)
        {
            SendAudioSettingsMessage settings = (SendAudioSettingsMessage)message;
            m_currentAudioSource.playOnAwake = settings.m_PlayOnAwake;
            m_currentAudioSource.loop = settings.m_CanLoop;
        }
    }
  
    public void OnDisableSubscribe()
    {
        throw new NotImplementedException();
    }
    public IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

        yield break;
    }
}
