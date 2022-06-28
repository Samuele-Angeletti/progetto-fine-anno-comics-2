using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName ="SO/AudioHolder")]
public class AudioHolder : ScriptableObject
{
    public Audio[] audioArray;
    private AudioMixer m_audioMixer;
    private void OnEnable()
    {
    }
    //private void OnValidate()
    //{
    //    m_audioMixer = Resources.Load("Assets / Audio / GameAudioMixer.mixer") as AudioMixer;

    //    if (audioArray != null)
    //    {
    //        for (int i = 0; i < audioArray.Length; i++)
    //        {
    //            if (audioArray[i].mixerGroup != null)
    //                audioArray[i].mixerGroup = m_audioMixer.FindMatchingGroups("BackGroundMusic").Single();
    //        }
    //    }



    //}
}
