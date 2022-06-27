using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent,]
public class AudioManager : MonoBehaviour 
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
        m_musicAudioSource = GetComponent<AudioSource>();
        m_musicAudioSource.outputAudioMixerGroup = m_audioMixer.FindMatchingGroups("BackGroundMusic").Single();

    }


    private void Start()
    {
        ChangeBackgroundMusic(ESoundTrackType.MusicaMenùPrincipale);
        PlayBackGroundMusic();
    }

    private void Update()
    {
        ChangeVolumeMusic(currentMusicVolume);
        ChangeVolumeSFX(currentSFXVolume);
    }


    #endregion

    private AudioSource m_musicAudioSource;
    
    [SerializeField] AudioMixer m_audioMixer;
    [SerializeField] Audio[] m_soundTracArray;
    [SerializeField, Range(-80f, 20f)] float currentSFXVolume;
    [SerializeField, Range(-80f, 20f)] float currentMusicVolume;
    [Space(10)]
    [SerializeField] List<AudioSource> m_audioPresentiInScena;



    private void OnValidate()
    {
        for (int i = 0; i < m_soundTracArray.Length; i++)
        {
            if (m_soundTracArray[i].mixerGroup == null)
                m_soundTracArray[i].mixerGroup = m_audioMixer.FindMatchingGroups("BackGroundMusic").Single();
        }
        

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
        m_audioMixer.SetFloat("Master", value);
    }
    public static AudioClip GetRandomAudioClip(AudioClip[] audioClips)
    {
        return  audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
    }
    public void PlayBackGroundMusic()
    {
        m_musicAudioSource.Play();
    }
    public void StopBackGroundMusic()
    {
        m_musicAudioSource.Stop();
    }
    public void ChangeBackgroundMusic(ESoundTrackType soundTrackType)
    {
        for (int i = 0; i < m_soundTracArray.Length; i++)
        {
            if (m_soundTracArray[i].soundTrack == soundTrackType)
            {
                m_musicAudioSource.clip = m_soundTracArray[i].audioToPlay;
            }
        }
    }
}
