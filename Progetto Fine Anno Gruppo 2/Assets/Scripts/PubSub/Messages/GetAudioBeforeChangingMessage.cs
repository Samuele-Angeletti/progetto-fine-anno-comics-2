using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class GetAudioBeforeChangingMessage : IMessage
{
    public AudioClip clip;
    public GetAudioBeforeChangingMessage(AudioClip clip)
    {
        this.clip = clip;
    }
}
