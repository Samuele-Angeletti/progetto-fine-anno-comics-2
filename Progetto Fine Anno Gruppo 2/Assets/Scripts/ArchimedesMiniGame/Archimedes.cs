using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
namespace ArchimedesMiniGame
{
    public class Archimedes : MonoBehaviour, ISubscriber
    {
        [SerializeField] SpriteRenderer m_ExternalSpriteRenderer;

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
        }

        public void OnPublish(IMessage message)
        {
            if(message is StartEngineModuleMessage)
            {
                m_ExternalSpriteRenderer.enabled = true;
            }
            else if(message is DockingCompleteMessage || message is NoBatteryMessage || message is ModuleDestroyedMessage)
            {
                m_ExternalSpriteRenderer.enabled = false;
            }
        }
    }
}
