using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
using MicroGame;

namespace Commons
{

    public class TimedObject : MonoBehaviour, ISubscriber
    {
        [SerializeField] float m_StartDelayTime;
        [SerializeField] float m_TimeActive;
        [SerializeField] float m_TimeNotActive;
        [SerializeField] bool m_StartNotActiveTimer;
        [SerializeField] GameObject m_ObjectToUse;
        [SerializeField] CheckPointLogic m_ActivatorCheckPoint;

        private float m_TimePassed;
        private bool m_Active;
        private bool m_StartDelay;
        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(CheckPointMessage));
            
        }

        private void Update()
        {
            if (m_Active)
            {
                if(m_StartDelay)
                {
                    Timer(m_StartDelayTime, m_StartNotActiveTimer);
                }
                else
                {
                    if (m_ObjectToUse.activeSelf)
                        Timer(m_TimeActive, false);
                    else
                        Timer(m_TimeNotActive, true);
                }
            }
        }

        private void Timer(float time, bool active)
        {
            m_TimePassed += Time.deltaTime;
            if (m_TimePassed >= time)
            {
                m_TimePassed = 0;
                m_ObjectToUse.SetActive(active);
                if (m_StartDelay) m_StartDelay = false;
            }
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(CheckPointMessage));
        }

        public void OnPublish(IMessage message)
        {
            if (message is CheckPointMessage)
            {
                CheckPointMessage checkPoint = (CheckPointMessage)message;
                if (checkPoint.CheckPointLogic == m_ActivatorCheckPoint)
                {
                    m_Active = true;
                    m_TimePassed = 0;
                    m_ObjectToUse.SetActive(!m_StartNotActiveTimer);
                    m_StartDelay = m_StartDelayTime > 0;
                }
                else
                {
                    m_Active = false;
                }
            }
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }


    }

}