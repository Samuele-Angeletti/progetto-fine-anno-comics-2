using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using ArchimedesMiniGame;

public class StartEngineModuleMessage : IMessage
{
    public Module Module;
    public Transform DestinationPoint;
    public StartEngineModuleMessage(Module module, Transform destinationPoint)
    {
        Module = module;
        DestinationPoint = destinationPoint;
    }
}
