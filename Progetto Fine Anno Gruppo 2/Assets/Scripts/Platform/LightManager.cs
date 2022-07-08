using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using ArchimedesMiniGame;
using Commons;
using MicroGame;
using UnityEngine.Rendering.Universal;
namespace MainGame
{
    public class LightManager : MonoBehaviour, ISubscriber
    {
        // MANAGES THE GLOBAL ILLUMINATION OF THE WORLD
        [SerializeField] Light2D m_GlobalLight;
        [SerializeField] int m_LayerOnPacMan;
        [SerializeField] float m_ExternalIntensity;
        [SerializeField] float m_InternalIntensity;
        [SerializeField] float m_PacManIntensity;
        int m_GlobalLightLayer;
        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(PlayPacManMessage));
            PubSub.PubSub.Subscribe(this, typeof(GameOverMicroGameMessage));
            PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(PlayPacManMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(GameOverMicroGameMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
        }

        public void OnPublish(IMessage message)
        {
            if(message is PlayPacManMessage)
            {
                m_GlobalLightLayer = m_GlobalLight.gameObject.layer;
                m_GlobalLight.gameObject.layer = m_LayerOnPacMan;
                SetGlobalLight(m_PacManIntensity);
            }
            else if(message is GameOverMicroGameMessage)
            {
                GameOverMicroGameMessage gameOverMicroGameMessage = (GameOverMicroGameMessage)message;
                if(gameOverMicroGameMessage.Win)
                    m_GlobalLight.gameObject.layer = m_GlobalLightLayer;

                SetGlobalLight(m_InternalIntensity);
            }
            else if(message is StartEngineModuleMessage)
            {
                SetGlobalLight(m_ExternalIntensity);
            }
            else if(message is DockingCompleteMessage || message is NoBatteryMessage || message is ModuleDestroyedMessage)
            {
                SetGlobalLight(m_InternalIntensity);
            }
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }

        public void SetGlobalLight(float amount)
        {
            m_GlobalLight.intensity = amount;
        }

    }
}
