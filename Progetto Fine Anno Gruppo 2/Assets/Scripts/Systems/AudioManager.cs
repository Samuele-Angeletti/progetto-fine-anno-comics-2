using System;
using System.Collections;
using System.Linq;
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

    [SerializeField, Range(0f, 1f)] float currentSFXVolume;
    [SerializeField, Range(0f, 1f)] float currentMusicVolume;


    private void Start()
    {
        PlayBackGroundMusic(backGroundMusic,"Musica Di BackGround");
    }

    public void PlayBackGroundMusic(AudioAssetSO musicToPlay,string objectName)
    {
        if (musicToPlay != null)
        {
            if (musicToPlay.audioType == EAudioType.BackGroundMusic)
            {
                if (m_backGroundMusic == null)
                {
                    m_backGroundMusic = new GameObject(objectName);
                    m_backGroundMusic.AddComponent<AudioSource>();
                    m_backGroundMusic.GetComponent<AudioSource>().playOnAwake = false;
                    m_backGroundMusic.GetComponent<AudioSource>().loop = true;
                    m_backGroundMusic.GetComponent<AudioSource>().clip = musicToPlay.clip;
                    m_backGroundMusic.GetComponent<AudioSource>().volume = 0;
                    m_backGroundMusic.GetComponent<AudioSource>().Play();


                }
                else if (m_backGroundMusic != null)
                {
                    m_backGroundMusic.GetComponent<AudioSource>().clip = musicToPlay.clip;
                    m_backGroundMusic.GetComponent<AudioSource>().Play();
                }
            }
            else
                Debug.LogError("The audio is not set to the correct audio type");

        }
        //else
            //Debug.Log("This scriptable object is without a audioClip");
    }

    public void PlayBackGroundMusic(AudioAssetSO audioAsset, string objectName, float delay)
    {
        if (audioAsset != null)
        {
            if (audioAsset.audioType == EAudioType.BackGroundMusic)
            {
                if (m_backGroundMusic == null)
                {
                    m_backGroundMusic = new GameObject(objectName);
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

    public void PlayBackGroundMusic(AudioAssetSO musicToPlay, string objectName, AnimationCurve curve)
    {
        if (musicToPlay != null)
        {
            if (musicToPlay.audioType == EAudioType.BackGroundMusic)
            {
                if (m_backGroundMusic == null)
                {
                    m_backGroundMusic = new GameObject(objectName);

                    m_backGroundMusic.AddComponent<AudioSource>();
                    m_backGroundMusic.GetComponent<AudioSource>().playOnAwake = false;
                    m_backGroundMusic.GetComponent<AudioSource>().loop = true;
                    m_backGroundMusic.GetComponent<AudioSource>().clip = musicToPlay.clip;
                    m_backGroundMusic.GetComponent<AudioSource>().volume = 0;
                    m_backGroundMusic.GetComponent<AudioSource>().Play();


                }
                else if (m_backGroundMusic != null)
                {
                    m_backGroundMusic.GetComponent<AudioSource>().clip = musicToPlay.clip;
                    m_backGroundMusic.GetComponent<AudioSource>().Play();
                }
            }
            else
                Debug.LogError("The audio is not set to the correct audio type");

        }
        else
            Debug.Log("This scriptable object is without a audioClip");
    }


    public void PlaySFX(AudioAssetSO sfxToPlay,string objectName, GameObject objectThatMakeTheSound,bool canLoop,bool is3DSound)
    {
        if (sfxToPlay != null)
        {
            switch (sfxToPlay.audioType)
            {
                case EAudioType.UISfx:
                    PlayGenericSFX(sfxToPlay, objectName, objectThatMakeTheSound, false,false);
                    break;
                case EAudioType.PlayerSFX:
                    PlayPlayerSFX();
                    break;
                case EAudioType.ObjectSFX:
                    PlayGenericSFX(sfxToPlay, objectName, objectThatMakeTheSound,canLoop,is3DSound);
                    break;
                case EAudioType.BackGroundMusic:
                    Debug.LogError("Per un'audio di tipo backGroundMusic bisogna utilizzare la funzione PlayBackGroundMusic()");
                    break;


            }
        }
        else
            Debug.LogError("The scriptableObject is null");

    }

    private void PlayGenericSFX(AudioAssetSO sfxToPlay,string objectName, GameObject objectThatMakeTheSound, bool canLoop, bool is3DSound)
    {
        if (objectThatMakeTheSound == null)
        {
            Debug.LogError("l'oggetto che deve emettere il suono è null");
            return;
        }
        else if (objectThatMakeTheSound.GetComponentInChildren<AudioSource>() == null)
        {
            //Spawning an object with audio source component
            GameObject audioSourceObject = new GameObject(objectName);
            audioSourceObject.AddComponent<AudioSource>();

            //Checking if the audio can loop
            if(canLoop) audioSourceObject.GetComponent<AudioSource>().loop = true;
            else audioSourceObject.GetComponent<AudioSource>().loop = false;

            if (is3DSound) audioSourceObject.GetComponent<AudioSource>().spatialBlend = 1;
            else audioSourceObject.GetComponent<AudioSource>().spatialBlend = 0;

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
    private static IEnumerator FadeAudioSource(AudioSource audioSource, float duration, float targetVolume)
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

    

    public void ChangeVolumeMusic(float volumeToChange)
    {
        if (m_backGroundMusic.GetComponent<AudioSource>() != null)
        {
            currentMusicVolume = volumeToChange;
        }
    }


    public void ChangeVolumeSFX(float volumeToChange)
    {
        if (m_Sfx.GetComponent<AudioSource>() != null)
        {
           currentSFXVolume = volumeToChange;
        }
    }
    
}
