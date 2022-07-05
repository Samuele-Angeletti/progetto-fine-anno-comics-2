using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using MainGame;
public class QuestCompleteMessage : IMessage
{
    public QuestSO QuestSO;

    public QuestCompleteMessage(QuestSO questSO)
    {
        QuestSO = questSO;
    }
}
