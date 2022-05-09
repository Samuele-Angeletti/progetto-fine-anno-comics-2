using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
public class ChangeContinousMovementMessage : IMessage
{
    public bool Active;

    public ChangeContinousMovementMessage(bool active)
    {
        Active = active;
    }
}
