using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAudioSettingsMessage : IMessage
{
    public bool m_CanLoop;
    public bool m_PlayOnAwake;
    public SendAudioSettingsMessage(bool CanLoop, bool PlayOnAwake)
    {
        m_CanLoop = CanLoop;
        m_PlayOnAwake = PlayOnAwake;
    }
}
