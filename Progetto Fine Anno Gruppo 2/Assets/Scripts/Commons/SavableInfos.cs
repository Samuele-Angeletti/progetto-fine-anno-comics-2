using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    [Serializable]
    public class SavableInfos
    {
        public float MaxLife;
        public float CurrentLife;
        public float MaxBattery;
        public float CurrentBattery;
        public float xPos;
        public float yPos;
        public float zPos;

        public SavableInfos(float maxLife, float currentLife, float maxBattery, float currentBattery, Vector3 currentPosition)
        {
            MaxLife = maxLife;
            CurrentLife = currentLife;
            MaxBattery = maxBattery;
            CurrentBattery = currentBattery;
            xPos = currentPosition.x;
            yPos = currentPosition.y;
            zPos = currentPosition.z;
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
