using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
public class PickablePickedMessage : IMessage
{
    public Pickable Pickable;

    public PickablePickedMessage(Pickable pickable)
    {
        this.Pickable = pickable;
    }
}
