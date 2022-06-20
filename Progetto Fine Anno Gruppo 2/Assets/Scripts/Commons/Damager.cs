using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Damager : MonoBehaviour
    {
        [SerializeField] float m_DamageAmount;
        [SerializeField] LayerMask m_LayerMask;

        public float DamageAmount => m_DamageAmount;
        public LayerMask LayerMask => m_LayerMask;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Damageable d = collision.collider.GetComponent<Damageable>();
            if (d != null && m_LayerMask.Contains(d.gameObject.layer))
            {
                d.TakeDamage(m_DamageAmount);
            }
        }

        public void Initialize(float damage, LayerMask layerMask)
        {
            m_DamageAmount = damage;
            m_LayerMask = layerMask;
        }

        
    }
}
