using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAudioType
{
    BackGroundMusic,
    PlayerSFX,
    ObjectSFX,
    UISfx
}

[CreateAssetMenu(fileName ="AudioAsset",menuName ="SO/AudioAsset")]
public class AudioAssetSO : ScriptableObject
{
    public EAudioType audioType;
    public AudioClip clip;
    public bool canFade;

}
