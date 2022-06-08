using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using ArchimedesMiniGame;

public class DockingCompleteMessage : IMessage
{
    public Module Module;

    public DockingCompleteMessage(Module module)
    {
        Module = module;
    }
}
