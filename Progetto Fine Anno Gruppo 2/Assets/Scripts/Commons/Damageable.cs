using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] float m_MaxLife;

        private float m_CurrentLife;
        private IDamageable m_DamageableHolder;

        public float MaxLife => m_MaxLife;
        public float CurrentLife => m_CurrentLife;


        private void Awake()
        {
            m_CurrentLife = m_MaxLife;
            m_DamageableHolder = GetComponent<IDamageable>();
        }

        internal void TakeDamage(float m_DamageAmount)
        {
            m_CurrentLife -= m_DamageAmount;

            if (m_DamageableHolder != null)
                m_DamageableHolder.GetDamage(m_CurrentLife);

            if (m_CurrentLife <= 0)
            {
                m_CurrentLife = 0;
                PubSub.PubSub.Publish(new ModuleDestroyedMessage());
            }
        }

    }
}
