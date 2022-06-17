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
    public AudioAssetSO backGroundMusic;
    private void Start()
    {
        //AudioManager.Instance.PlayBackGroundMusic(backGroundMusic);
    }

    public void PlayBackGroundMusic(AudioAssetSO musicToPlay)
    {
        //if (musicToPlay != null)
        //{
        //    if (musicToPlay.audioType == EAudioType.BackGroundMusic)
        //    {
        //        if (m_backGroundMusic == null)
        //        {
        //            m_backGroundMusic = new GameObject("BackGroundMusic");
        //            m_backGroundMusic.AddComponent<AudioSource>();
        //            m_backGroundMusic.GetComponent<AudioSource>().playOnAwake = false;
        //            m_backGroundMusic.GetComponent<AudioSource>().loop = true;
        //            m_backGroundMusic.GetComponent<AudioSource>().clip = musicToPlay.clip;
        //            m_backGroundMusic.GetComponent<AudioSource>().Play();


        //        }
        //        else if (m_backGroundMusic != null)
        //        {
        //            m_backGroundMusic.GetComponent<AudioSource>().clip = musicToPlay.clip;
        //            m_backGroundMusic.GetComponent<AudioSource>().Play();
        //        }
        //    }
        //    else
        //        Debug.LogError("The audio is not set to the correct audio type");

        //}
        //else
        //    Debug.LogError("This scriptable object is without a audioClip");
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
                Debug.LogErrorFormat("The scriptableObject is null");

        }
        else
            Debug.LogError("This scriptable object is without a audioClip");
    }
    public void PlaySFX(AudioAssetSO sfxToPlay,GameObject objectThatMakeTheSound,bool canLoop)
    {
        if (sfxToPlay != null)
        {
            switch (sfxToPlay.audioType)
            {
                case EAudioType.UISfx:
                    PlayUISFX();
                    break;
                case EAudioType.PlayerSFX:
                    PlayPlayerSFX();
                    break;
                case EAudioType.ObjectSFX:
                    PlayObjectSFX(sfxToPlay,objectThatMakeTheSound,canLoop);
                    break;
                case EAudioType.BackGroundMusic:
                    Debug.LogError("Per un'audio di tipo backGroundMusic bisogna utilizzare la funzione PlayBackGroundMusic()");
                    break;


            }
        }
        else
            Debug.LogError("The scriptableObject is null");

    }

    private void PlayObjectSFX(AudioAssetSO sfxToPlay,GameObject objectThatMakeTheSound, bool canLoop)
    {
        if (objectThatMakeTheSound == null)
        {
            Debug.LogError("l'oggetto che deve emettere il suono è null");
            return;
        }
        else if (objectThatMakeTheSound.GetComponentInChildren<AudioSource>() == null)
        {
            //Spawning an object with audio source component
            GameObject audioSourceObject = new GameObject("ObjectSFX");
            audioSourceObject.AddComponent<AudioSource>();

            //Checking if the audio can loop
            if(canLoop) audioSourceObject.GetComponent<AudioSource>().loop = true;
            else audioSourceObject.GetComponent<AudioSource>().loop = false;

            //Making the spawnedObject child with the objectThatMakeTheSound and giving them the same position
            audioSourceObject.transform.SetParent(objectThatMakeTheSound.transform);
            audioSourceObject.transform.position = objectThatMakeTheSound.transform.position;

            audioSourceObject.GetComponent<AudioSource>().PlayOneShot(sfxToPlay.clip);


        }
        else if (objectThatMakeTheSound.GetComponentInChildren<AudioSource>() != null)
        {
            AudioSource temp = objectThatMakeTheSound.GetComponentInChildren<AudioSource>();
            temp.PlayOneShot(sfxToPlay.clip);
        }
    }

    private void PlayPlayerSFX()
    {
        throw new NotImplementedException();
    }

    private void PlayUISFX()
    {
        throw new NotImplementedException();
    }

    public void ChangeVolumeMusic(float volumeToChange)
    {
        if (m_backGroundMusic.GetComponent<AudioSource>() != null)
        {
            m_backGroundMusic.GetComponent<AudioSource>().volume = volumeToChange;
        }
    }


    public void ChangeVolumeSFX(float volumeToChange)
    {
        if (m_Sfx.GetComponent<AudioSource>() != null)
        {
            m_Sfx.GetComponent<AudioSource>().volume = volumeToChange;
        }
    }
    
}
