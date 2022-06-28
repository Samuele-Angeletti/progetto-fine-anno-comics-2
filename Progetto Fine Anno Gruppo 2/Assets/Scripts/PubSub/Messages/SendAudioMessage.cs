using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class SendAudioMessage : IMessage
{

    public AudioHolder audioToSend;
    public SendAudioMessage(AudioHolder audioToSend)
    {
     this.audioToSend = audioToSend;
    }
}
