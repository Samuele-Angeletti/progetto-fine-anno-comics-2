using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;
using ArchimedesMiniGame;
using MicroGame;
using System;

namespace MainGame
{
    public class QuestSystem : MonoBehaviour, ISubscriber
    {
        [SerializeField] List<QuestSO> m_QuestsList;
        QuestSO m_CurrentQuest;

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(QuestCompleteMessage));
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }

        public void OnPublish(IMessage message)
        {
            if(message is QuestCompleteMessage)
            {
                QuestCompleteMessage questCompleteMessage = (QuestCompleteMessage)message;
                CheckQuest(questCompleteMessage.QuestSO);
            }
        }

        private void CheckQuest(QuestSO currentQuest)
        {
            if(m_CurrentQuest == currentQuest)
            {
                if (!currentQuest.QuestAccomplished)
                {
                    m_CurrentQuest.QuestAccomplished = true;

                    if (m_QuestsList.Find(x => !x.QuestAccomplished) == null)
                    {
                        UIManager.Instance.SetQuest("");
                        GameManager.Instance.GameOver();
                    }
                    else
                        SetNextQuest(m_QuestsList.IndexOf(currentQuest) + 1);
                }
            }
            else if(m_QuestsList.Contains(currentQuest))
            {
                currentQuest.QuestAccomplished = true;
            }
        }

        public void SetNextQuest(int index)
        {
            m_CurrentQuest = m_QuestsList[index];
            m_CurrentQuest.QuestAccomplished = false;
            UIManager.Instance.SetQuest(m_CurrentQuest.MessageOnScreen);
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(QuestCompleteMessage));
        }


    }
}
