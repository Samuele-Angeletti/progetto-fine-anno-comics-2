using ArchimedesMiniGame;
using MainGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Commons
{
    public class IntegrationEvents : MonoBehaviour
    {
        [SerializeField] List<IntegrationEventScriptableObject> integrationEventsSO;
        [SerializeField] UnityEvent sceneEvents;
        public void Invoke()
        {
            foreach(IntegrationEventScriptableObject ie in integrationEventsSO)
            {
                ie.Invoke(GameManager.Instance, GameManagerES.Instance);
            }

            sceneEvents.Invoke();


        }
    }
}