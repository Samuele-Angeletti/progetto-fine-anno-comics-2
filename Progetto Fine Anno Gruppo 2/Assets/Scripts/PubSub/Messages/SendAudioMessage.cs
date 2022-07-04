using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class SendAudioMessage : IMessage
{

    public AudioClip audioClipToSend;
    public SendAudioMessage(AudioClip audioToSend)
    {
        audioClipToSend = audioToSend;
    }
}
