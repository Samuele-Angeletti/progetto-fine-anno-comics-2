using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;
using MainGame;
using System;

namespace ArchimedesMiniGame
{
    public class Capsula : MonoBehaviour, ISubscriber
    {
        [SerializeField] bool m_PlayOnStart;
        [SerializeField] PlayerMovementManager m_PlayerPrefab;
        [SerializeField] Transform m_SpawnPoint;
        private Module m_Module;

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
        }

        public void OnPublish(IMessage message)
        {
            if(message is DockingCompleteMessage)
            {
                DockingCompleteMessage dockingCompleteMessage = (DockingCompleteMessage)message;
                if(dockingCompleteMessage.Module == m_Module)
                {
                    SpawnPlayer();
                    Invoke("OnDisableSubscribe", 1f);
                }
            }
        }

        private void SpawnPlayer()
        {
            PlayerMovementManager player = Instantiate(m_PlayerPrefab, m_SpawnPoint.position, Quaternion.identity);
            GameManager.Instance.SetNewPlayer(player);
        }

        private void Awake()
        {
            m_Module = GetComponent<Module>();
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));

            if(m_PlayOnStart)
                Invoke("DebugEngine", 0.5f);
        }

        private void DebugEngine()
        {

            m_Module.TryStartEngine();
        }
    }
}