using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public struct Audio
{
    public ESoundTrackType soundTrack;
    public AudioClip musicToPlay;
    public AudioMixerGroup mixerGroup;
}

public enum ESoundTrackType
{
    MusicaMenuPrincipale,
    MusicaIntroduzione,
    MusicaPrimoAttracco,
    
}