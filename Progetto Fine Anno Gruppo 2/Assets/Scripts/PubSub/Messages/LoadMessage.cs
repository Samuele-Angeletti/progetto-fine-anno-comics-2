using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;

public class LoadMessage : IMessage
{
    public Dictionary<string, SavableInfos> Database;

    public LoadMessage(Dictionary<string, SavableInfos> database)
    {
        Database = database;
    }
}
