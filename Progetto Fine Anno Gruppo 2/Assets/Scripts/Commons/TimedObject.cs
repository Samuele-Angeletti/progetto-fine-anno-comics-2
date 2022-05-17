using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
using MicroGame;

namespace Commons
{

    public class TimedObject : Activable, ISubscriber
    {
        [Header("Timed Object Settings")]
        [SerializeField] float m_StartDelayTime;
        [SerializeField] float m_TimeActive;
        [SerializeField] float m_TimeNotActive;
        [SerializeField] bool m_StartNotActiveTimer;
        [SerializeField] GameObject m_ObjectToUse;

        private float m_TimePassed;
        private bool m_Active;
        private bool m_StartDelay;
        private bool m_ActiveOnStart;
        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(ActivableMessage));
            OnStartVariablesReferredCheck();
            m_ActiveOnStart = m_ObjectToUse.activeSelf;
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
            PubSub.PubSub.Unsubscribe(this, typeof(ActivableMessage));
        }

        public void OnPublish(IMessage message)
        {
            if (message is ActivableMessage)
            {
                ActivableMessage activableMessage = (ActivableMessage)message;
                if (activableMessage.Activator == Activator && activableMessage.Active)
                {
                    OnActive();
                }
                else
                {
                    OnDeactive();
                }
            }
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }

        public override void OnActive()
        {
            m_Active = true;
            m_TimePassed = 0;
            m_ObjectToUse.SetActive(!m_StartNotActiveTimer);
            m_StartDelay = m_StartDelayTime > 0;
        }

        public override void OnDeactive()
        {
            m_Active = false;
            m_ObjectToUse.SetActive(m_ActiveOnStart);
        }

        public override void OnStartVariablesReferredCheck()
        {
            if (Activator == null)
            {
                Debug.LogError($"L'oggetto {gameObject.name} non contiene un Activator!");
            }
        }

    }

}