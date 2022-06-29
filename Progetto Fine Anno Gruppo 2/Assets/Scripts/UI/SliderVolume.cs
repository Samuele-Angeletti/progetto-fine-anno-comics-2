using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    [SerializeField]AudioMixer mixer;
    [SerializeField] string MixerGroupToTarget;


    public void SetFloat(float value)
    {
        if (mixer != null)
        {
            mixer.SetFloat(MixerGroupToTarget,Mathf.Log10(value)*20);
        }
    }

}
