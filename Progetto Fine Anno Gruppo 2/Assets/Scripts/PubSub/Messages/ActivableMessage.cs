using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class ActivableMessage : IMessage
{
    public bool Active;
    public Activator Activator;
    
    public ActivableMessage(bool active, Activator activator)
    {
        Activator = activator;
        Active = active;
    }
}
