using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    public Slider m_volumeSlider;

    private void Start()
    {
       AudioManager.Instance.ChangeVolumeMusic(m_volumeSlider.value);
    }

}
