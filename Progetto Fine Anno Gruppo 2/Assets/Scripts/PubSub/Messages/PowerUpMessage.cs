using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
public class PowerUpMessage : IMessage
{
    public bool Active;

    public PowerUpMessage(bool active)
    {
        Active = active;
    }
}
