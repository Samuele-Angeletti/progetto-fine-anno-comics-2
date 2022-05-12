using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
public class CheckPointMessage : IMessage
{
    public Vector3 Position;
    public CheckPointLogic CheckPointLogic;
    public CheckPointMessage(Vector3 position, CheckPointLogic checkPointLogic)
    {
        Position = position;
        CheckPointLogic = checkPointLogic;
    }
}
