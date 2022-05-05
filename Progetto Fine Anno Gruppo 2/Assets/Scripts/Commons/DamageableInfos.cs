using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    [Serializable]
    public class DamageableInfos
    {
        public float MaxLife;
        public float CurrentLife;

        public DamageableInfos(float maxLife, float currentLife)
        {
            MaxLife = maxLife;
            CurrentLife = currentLife;
        }

        /// <summary>
        /// Danno compreso tra 0 e 1 in percentuale. 0 = Nessun danno, 1 = danneggiato al 100%.
        /// </summary>
        /// <returns></returns>
        public float DamagePercentage()
        {
            return (MaxLife - CurrentLife) / MaxLife;
        }
    }
}
