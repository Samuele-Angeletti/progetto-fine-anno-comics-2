using ArchimedesMiniGame;
using MainGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public class IntegrationEvents : MonoBehaviour
    {
        [SerializeField] List<IntegrationEventScriptableObject> integrationEventsSO;

        public void Invoke()
        {
            foreach(IntegrationEventScriptableObject ie in integrationEventsSO)
            {
                ie.Invoke(GameManager.Instance, GameManagerES.Instance);
            }
        }
    }
}