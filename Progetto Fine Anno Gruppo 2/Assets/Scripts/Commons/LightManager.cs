using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using PubSub;
namespace Commons
{
    public class LightManager : MonoBehaviour, ISubscriber
    {

        public void OnDisableSubscribe()
        {

        }

        public void OnPublish(IMessage message)
        {

        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }
    }
}

