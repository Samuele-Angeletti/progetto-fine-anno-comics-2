using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        
    }

   





    #endregion

    private AudioSource m_musicAudioSource;
    
    [SerializeField] AudioMixer m_audioMixer;
    [SerializeField] List<AudioMixerGroup> m_groups;
    [SerializeField, Range(0f, 1f)] float currentSFXVolume;
    [SerializeField, Range(0f, 1f)] float currentMusicVolume;
    [Space(10)]
    [SerializeField] List<AudioSource> m_audioPresentiInScena;


    
    private void GetAllAudioSourceInCurrentScene(Scene scene, LoadSceneMode mode)
    {
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            m_audioPresentiInScena.Add(audio);
        }
    }
    private void RemoveAllAudioSourceInCurrentScene(Scene scene)
    {
        m_groups.Clear();
    }
    private void OnValidate()
    {
        if (m_audioMixer != null)
        {
            foreach (var mixerGroups in m_audioMixer.FindMatchingGroups("Master"))
            {
                if (!m_groups.Find(x => x == mixerGroups))
                {
                    m_groups.Add(mixerGroups);
                }

            }
        }
        else if (m_audioMixer == null)
        {
            m_groups.Clear();
        }
       
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += GetAllAudioSourceInCurrentScene;
        SceneManager.sceneUnloaded += RemoveAllAudioSourceInCurrentScene;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= GetAllAudioSourceInCurrentScene;
        SceneManager.sceneUnloaded -= RemoveAllAudioSourceInCurrentScene;
    }
    public void ChangeVolumeSFX(float value)
    {
        m_audioMixer.SetFloat("SFX",Mathf.Log10(value)*20);
    }
    public void ChangeVolumeMusic(float value)
    {
        m_audioMixer.SetFloat("BackGroundMusic",value);
    
    }
    public void ChangeVolumeAmbience(float value)
    {
        m_audioMixer.SetFloat("AmbientSound", value);

    }
    public void ChangeMasterVolume(float value)
    {
        m_audioMixer.SetFloat("Master", value);
    }

}
