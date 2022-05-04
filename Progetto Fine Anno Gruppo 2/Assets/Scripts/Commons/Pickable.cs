using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
namespace Commons
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField] EMessageType m_MessageType;

        public void Picked()
        {
            PubSub.PubSub.Publish(new PickablePickedMessage(this));

            SwitcherSystem.SwitchMessageType(m_MessageType,
                () => PubSub.PubSub.Publish(new GameOverMicroGameMessage()), // to replace if gameOver can be used from pickables even in other situations
                () => PubSub.PubSub.Publish(new PowerUpMessage(true)));

            Destroy(gameObject);
        }
    }
}

