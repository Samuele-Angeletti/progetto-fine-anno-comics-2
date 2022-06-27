using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public struct Audio
{
    public ESoundTrackType soundTrack;
    public AudioClip audioToPlay;
    public AudioMixerGroup mixerGroup;
}

public enum ESoundTrackType
{
    MusicaMenùPrincipale,
    MusicaIntroduzione,
    MusicaPrimoAtracco,
    
}