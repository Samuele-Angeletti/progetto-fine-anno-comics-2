using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
using Cinemachine;
using MainGame;
using UnityEngine.Tilemaps;

namespace ArchimedesMiniGame
{
    public class Archimedes : MonoBehaviour, ISubscriber
    {
        [SerializeField] SpriteRenderer m_ExternalSpriteRenderer;
        [SerializeField] CinemachineVirtualCamera m_MyCameraOnPlayer;
        [SerializeField] GameObject m_Terrain;

        private PolygonCollider2D m_PolygonCollider;
        private SpriteTransition m_SpriteTransition;
        private void Awake()
        {
            m_SpriteTransition = GetComponent<SpriteTransition>();
            m_PolygonCollider = GetComponent<PolygonCollider2D>();
        }
        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
            m_ExternalSpriteRenderer.gameObject.SetActive(true);
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
                m_Terrain.SetActive(false);
                m_PolygonCollider.enabled = true;
                m_SpriteTransition.ActiveSpriteTransparencyTransition(m_ExternalSpriteRenderer, EDirection.Up);
            }
            else if(message is DockingCompleteMessage || message is NoBatteryMessage || message is ModuleDestroyedMessage)
            {
                m_Terrain.SetActive(true);
                m_PolygonCollider.enabled = false;
                m_SpriteTransition.ActiveSpriteTransparencyTransition(m_ExternalSpriteRenderer, EDirection.Down);
            }
        }

        public void SetCameraOnGameManager()
        {
            GameManager.Instance.SetCameraOnPlayer(m_MyCameraOnPlayer);
        }
    }
}
