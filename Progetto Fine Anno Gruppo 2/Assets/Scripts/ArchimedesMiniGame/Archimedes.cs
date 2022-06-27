using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
using Cinemachine;
using MainGame;

namespace ArchimedesMiniGame
{
    public class Archimedes : MonoBehaviour, ISubscriber
    {
        [SerializeField] SpriteRenderer m_ExternalSpriteRenderer;
        [SerializeField] CinemachineVirtualCamera m_MyCameraOnPlayer;
        private SpriteTransition m_SpriteTransition;
        private void Awake()
        {
            m_SpriteTransition = GetComponent<SpriteTransition>();
        }
        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));

        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));
        }

        public void OnPublish(IMessage message)
        {
            if(message is StartEngineModuleMessage)
            {
                m_SpriteTransition.ActiveSpriteTransparencyTransition(m_ExternalSpriteRenderer, EDirection.Up);
            }
            else if(message is DockingCompleteMessage || message is NoBatteryMessage || message is ModuleDestroyedMessage)
            {
                m_SpriteTransition.ActiveSpriteTransparencyTransition(m_ExternalSpriteRenderer, EDirection.Down);
            }
        }

        public void SetCameraOnGameManager()
        {
            GameManager.Instance.SetCameraOnPlayer(m_MyCameraOnPlayer);
        }
    }
}
