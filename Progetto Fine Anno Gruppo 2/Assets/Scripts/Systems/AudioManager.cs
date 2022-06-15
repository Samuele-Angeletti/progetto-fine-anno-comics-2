using System;
using UnityEngine;

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
    private GameObject m_backGroundMusic;
    private GameObject m_Sfx;

   
    public void PlayBackGroundMusic(AudioAssetSO audioAsset)
    {
        if (audioAsset != null)
        {
            if (audioAsset != null)
            {
                if (audioAsset.audioType == EAudioType.BackGroundMusic)
                {
                    if (m_backGroundMusic == null)
                    {
                        m_backGroundMusic = new GameObject("BackGroundMusic");
                        m_backGroundMusic.AddComponent<AudioSource>();
                        m_backGroundMusic.GetComponent<AudioSource>().playOnAwake = false;
                        m_backGroundMusic.GetComponent<AudioSource>().loop = true;
                        m_backGroundMusic.GetComponent<AudioSource>().clip = audioAsset.clip;
                        m_backGroundMusic.GetComponent<AudioSource>().Play();


                    }
                    else if (m_backGroundMusic != null)
                    {
                        m_backGroundMusic.GetComponent<AudioSource>().clip = audioAsset.clip;
                        m_backGroundMusic.GetComponent<AudioSource>().Play();
                    }
                }
                else
                    Debug.LogError("The audio is not set to the correct audio type");

            }
            else
                Debug.LogError("This scriptable object is without a audioClip");
        }
    }
    public void PlayBackGroundMusic(AudioAssetSO audioAsset, float delay)
    {
        if (audioAsset != null)
        {
            if (audioAsset.audioType == EAudioType.BackGroundMusic)
            {
                if (m_backGroundMusic == null)
                {
                    m_backGroundMusic = new GameObject("BackGroundMusic");
                    m_backGroundMusic.AddComponent<AudioSource>();
                    m_backGroundMusic.GetComponent<AudioSource>().playOnAwake = false;
                    m_backGroundMusic.GetComponent<AudioSource>().loop = true;
                    m_backGroundMusic.GetComponent<AudioSource>().clip = audioAsset.clip;
                    m_backGroundMusic.GetComponent<AudioSource>().PlayDelayed(delay);


                }
                else if (m_backGroundMusic != null)
                {
                    m_backGroundMusic.GetComponent<AudioSource>().clip = audioAsset.clip;
                    m_backGroundMusic.GetComponent<AudioSource>().PlayDelayed(delay);
                }
            }
            else
                Debug.LogErrorFormat("The audio is not set to the correct audio type");

        }
        else
            Debug.LogError("This scriptable object is without a audioClip");
    }
    public void ChangeVolumeMusic(float volumeToChange)
    {
        if (m_backGroundMusic == null) return;
        m_backGroundMusic.GetComponent<AudioSource>().volume = volumeToChange;

    }
}
