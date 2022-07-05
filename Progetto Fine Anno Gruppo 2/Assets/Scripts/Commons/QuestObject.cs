using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class QuestObject : MonoBehaviour
{
    [SerializeField] QuestSO m_Quest;
    
    public void QuestCompleted()
    {
        PubSub.PubSub.Publish(new QuestCompleteMessage(m_Quest));
    }
}
