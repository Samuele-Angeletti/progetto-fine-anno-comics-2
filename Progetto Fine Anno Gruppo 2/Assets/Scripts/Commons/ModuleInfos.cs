using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    [Serializable]
    public class ModuleInfos
    {
        public float MaxLife;
        public float CurrentLife;
        public float MaxBattery;
        public float CurrentBattery;

        public ModuleInfos(float maxLife, float currentLife, float maxBattery, float currentBattery)
        {
            MaxLife = maxLife;
            CurrentLife = currentLife;
            MaxBattery = maxBattery;
            CurrentBattery = currentBattery;
        }

        /// <summary>
        /// Danno compreso tra 0 e 1 in percentuale. 0 = Nessun danno, 1 = danneggiato al 100%.
        /// </summary>
        /// <returns></returns>
        public float DamagePercentage()
        {
            return (MaxLife - CurrentLife) / MaxLife;
        }

        public void SetCurrentLife(float amount)
        {
            CurrentLife = Mathf.Clamp(amount, 0, MaxLife);
        }
    }
}
