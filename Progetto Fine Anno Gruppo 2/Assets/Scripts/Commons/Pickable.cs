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
                null,
                () => PubSub.PubSub.Publish(new PowerUpMessage(true)),
                null,
                null,
                null);

            Destroy(gameObject);
        }
    }
}

