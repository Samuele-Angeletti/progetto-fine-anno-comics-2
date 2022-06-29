using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class SendAudioMessage : IMessage
{

    public AudioHolder audioHolderToSend;
    public SendAudioMessage(AudioHolder audioToSend)
    {
     this.audioHolderToSend = audioToSend;
    }
}
