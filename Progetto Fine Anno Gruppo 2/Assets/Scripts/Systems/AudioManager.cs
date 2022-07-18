using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using PubSub;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
        m_AudioSourceMusica.outputAudioMixerGroup = m_audioMixer.FindMatchingGroups("BackGroundMusic").Single();
        m_AudioSourceAmbience.outputAudioMixerGroup = m_audioMixer.FindMatchingGroups("AmbientSound").Single();



    }


    #endregion

    [Header("MENU MUSIC")]
    [SerializeField] AudioClip m_musicaMenùPrincipale;
    [ReadOnly]public AudioClip musicaDaSuonare;
    [Header("AUDIO REFERENCE")]
    [SerializeField] AudioSource m_AudioSourceMusica;
    [SerializeField] AudioSource m_AudioSourceAmbience;
    [SerializeField] AudioMixer m_audioMixer;
    [Header("FADING SETTINGS")]
    public float fadeInTime;
    public float fadeOutTime;
   
    [SerializeField,ReadOnly] bool m_courutineIsStarted = false;





    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(SendAudioSettingsMessage));
        PubSub.PubSub.Subscribe(this, typeof(OnTriggerEnterAudio));
        PubSub.PubSub.Subscribe(this, typeof(SendAudioMessage));
        PubSub.PubSub.Subscribe(this, typeof(GetAudioBeforeChangingMessage));
        PubSub.PubSub.Subscribe(this, typeof(PauseGameMessage));
        PubSub.PubSub.Subscribe(this,typeof(ResumeGameMessage));
        PubSub.PubSub.Publish(new SendAudioSettingsMessage(true,true));
        PubSub.PubSub.Publish(new SendAudioMessage(m_musicaMenùPrincipale));
        if (m_musicaMenùPrincipale == null) return;


       

    }




    #region FOR_SETTINGS
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
    #endregion
    public void ChangeMusicAndPlay(AudioClip clip)
    {
        PubSub.PubSub.Publish(new SendAudioMessage(clip));
    }


    public void OnPublish(IMessage message)
    {

        if (message is SendAudioMessage)
        {
            SendAudioMessage audio = (SendAudioMessage)message;
            musicaDaSuonare = audio.audioClipToSend;
            if (m_AudioSourceMusica.clip != null)
            {
                this.RunCoroutine(FadeOutAndIn(musicaDaSuonare));
            }
            else if (m_AudioSourceMusica.clip == null)
            {
                m_AudioSourceMusica.clip = musicaDaSuonare;
                m_AudioSourceMusica.Play();
                this.RunCoroutine(WaitForFade(m_AudioSourceMusica, fadeOutTime, 1));


            }

        }
        else if (message is SendAudioSettingsMessage)
        {
            SendAudioSettingsMessage settings = (SendAudioSettingsMessage)message;
            m_AudioSourceMusica.playOnAwake = settings.m_PlayOnAwake;
            m_AudioSourceMusica.loop = settings.m_CanLoop;
        }
        else if (message is PauseGameMessage)
        {
            m_AudioSourceMusica.Stop();
        }
        else if (message is ResumeGameMessage)
        {
            m_AudioSourceMusica.Play();
        }
    }

   

    private IEnumerator FadeOutAndIn(AudioClip audio)
    {
        var coroutine = this.RunCoroutine(WaitForFade(m_AudioSourceMusica,audio, fadeOutTime, 0));
        yield return new WaitUntil(() => coroutine.IsDone);
        this.RunCoroutine(WaitForFade(m_AudioSourceMusica, fadeInTime, 1));
        
    }




    public IEnumerator WaitForFade(AudioSource audioSource,AudioClip audio, float duration, float targetVolume)
    {
       
        var courutine = this.RunCoroutine(StartFade(audioSource, duration, targetVolume));
        yield return new WaitUntil(() =>audioSource.volume == targetVolume);
        audioSource.clip = audio;
        audioSource.Play();
        m_courutineIsStarted = false;
    }
    public IEnumerator WaitForFade(AudioSource audioSource, float duration, float targetVolume)
    {

        var courutine = this.RunCoroutine(StartFade(audioSource, duration, targetVolume));
        yield return new WaitUntil(() => audioSource.volume == targetVolume);
        m_courutineIsStarted=false;
    }





    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        m_courutineIsStarted = true;
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
    public static AudioClip GetRandomAudioClip(List<AudioClip> clipList)
    {
        if (clipList == null) return null;
        int getRandomIndex = UnityEngine.Random.Range(0, clipList.Count);
        return clipList[getRandomIndex];
    }
    public static AudioClip GetRandomAudioClip(AudioClip[] clips)
    {
        if(clips == null) return null;
        int getRandomIndex = UnityEngine.Random.Range(0,clips.Length);
        return clips[getRandomIndex];
    }

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(OnTriggerEnterAudio));
        PubSub.PubSub.Unsubscribe(this, typeof(SendAudioMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(SendAudioSettingsMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(GetAudioBeforeChangingMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(PauseGameMessage));

    }


}
