using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using ArchimedesMiniGame;

public class StartEngineModuleMessage : IMessage
{
    public Module Module;

    public StartEngineModuleMessage(Module module)
    {
        Module = module;
    }
}
