using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class GameOverMicroGameMessage : IMessage
{
    public bool Win;
    
    public GameOverMicroGameMessage(bool win)
    {
        Win = win;
    }
}
