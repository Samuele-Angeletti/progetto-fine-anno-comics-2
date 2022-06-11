using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] float m_MaxLife;
        [SerializeField] EMessageType m_MessageType;
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
                SwitcherSystem.SwitchMessageType(m_MessageType, null, null, null, () => PubSub.PubSub.Publish(new ModuleDestroyedMessage()), null, () => PubSub.PubSub.Publish(new PlayerDeathMessage()));
            }
        }

        public void SetInitialLife(float amount)
        {
            m_CurrentLife = Mathf.Clamp(amount, 1, MaxLife);
        }

        public void SetMaxLife(float amount)
        {
            m_MaxLife = amount;
        }
    }
}
