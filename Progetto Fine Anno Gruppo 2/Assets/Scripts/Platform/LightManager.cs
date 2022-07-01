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

        int m_GlobalLightLayer;

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(PlayPacManMessage));
            PubSub.PubSub.Subscribe(this, typeof(GameOverMicroGameMessage));
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(PlayPacManMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(GameOverMicroGameMessage));
        }

        public void OnPublish(IMessage message)
        {
            if(message is PlayPacManMessage)
            {
                m_GlobalLightLayer = m_GlobalLight.gameObject.layer;
                m_GlobalLight.gameObject.layer = m_LayerOnPacMan;
            }
            else if(message is GameOverMicroGameMessage)
            {
                GameOverMicroGameMessage gameOverMicroGameMessage = (GameOverMicroGameMessage)message;
                if(gameOverMicroGameMessage.Win)
                    m_GlobalLight.gameObject.layer = m_GlobalLightLayer;
            }
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }
    }
}
