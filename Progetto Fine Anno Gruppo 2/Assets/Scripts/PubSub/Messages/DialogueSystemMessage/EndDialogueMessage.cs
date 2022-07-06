using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class EndDialogueMessage : IMessage
{
    public GameObject gameObject;
    public EndDialogueMessage(GameObject gameobjectToDestroy)
    {
        gameObject = gameobjectToDestroy;
    }
}
