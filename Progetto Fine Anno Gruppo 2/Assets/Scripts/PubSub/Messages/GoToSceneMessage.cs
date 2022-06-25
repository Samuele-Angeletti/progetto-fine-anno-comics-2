using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
public class GoToSceneMessage : IMessage
{
    public string SceneName;

    public GoToSceneMessage(string sceneName)
    {
        SceneName = sceneName;
    }
}
