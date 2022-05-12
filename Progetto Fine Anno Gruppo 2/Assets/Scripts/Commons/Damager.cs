using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class Damager : MonoBehaviour
    {
        [SerializeField] float m_DamageAmount;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Damageable d = collision.collider.GetComponent<Damageable>();
            if (d != null)
            {
                d.TakeDamage(m_DamageAmount);
            }
        }
    }
}
