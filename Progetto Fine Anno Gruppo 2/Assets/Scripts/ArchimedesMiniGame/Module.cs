using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
namespace ArchimedesMiniGame
{
    public class Module : Controllable, IDamageable
    {
        [Header("Main settings")]
        [SerializeField] float m_Acceleration;
        [SerializeField] float m_MaxSpeed;
        [SerializeField] float m_RotationSpeed;
        [SerializeField] GameObject m_DockingSide;
        [Space(10)]
        [Header("Battery Settings")]
        [SerializeField] float m_MaxBattery;
        [SerializeField] float m_UseSpeed;

        private float m_CurrentBattery;
        private Rigidbody2D m_Rigidbody;
        private Vector2 m_RotationDirection;
        private Damageable m_Damageable;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_CurrentBattery = m_MaxBattery;
            m_Damageable = GetComponent<Damageable>();
        }

        private void Start()
        {
            GameManagerES.Instance.UpdateBatterySlider(m_CurrentBattery, m_MaxBattery);
            GameManagerES.Instance.UpdateLifeSlider(m_Damageable.CurrentLife, m_Damageable.MaxLife);
        }

        private void FixedUpdate()
        {
            m_Rigidbody.velocity += -m_Direction.normalized * m_Acceleration * Time.fixedDeltaTime;
            m_Rigidbody.rotation += m_RotationDirection.x * Time.fixedDeltaTime * m_RotationSpeed;
        }

        private void Update()
        {
            if(m_Direction != Vector2.zero || m_RotationDirection != Vector2.zero)
            {
                UseBattery();
            }

            if (m_Rigidbody.velocity.x >= m_MaxSpeed)
            {
                m_Rigidbody.velocity = new Vector2(m_MaxSpeed, m_Rigidbody.velocity.y);
            }

            if (m_Rigidbody.velocity.y >= m_MaxSpeed)
            {
                m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_MaxSpeed);
            }
        }

        public override void MoveRotation(Vector2 newDirection)
        {
            m_Rigidbody.freezeRotation = true;
            m_Rigidbody.freezeRotation = false;
            if (m_CurrentBattery > 0)
            {
                m_RotationDirection = newDirection.normalized;
            }
        }

        public override void MoveDirection(Vector2 newDirection)
        {
            if (m_CurrentBattery > 0)
            {
                m_Direction = transform.forward - new Vector3(newDirection.x, newDirection.y);
            }
        }

        private void UseBattery()
        {
            m_CurrentBattery -= m_UseSpeed * Time.deltaTime;

            GameManagerES.Instance.UpdateBatterySlider(m_CurrentBattery, m_MaxBattery);

            if(m_CurrentBattery <= 0)
            {
                m_CurrentBattery = 0;
                GameManagerES.Instance.UpdateBatterySlider(m_CurrentBattery, m_MaxBattery);
                PubSub.PubSub.Publish(new NoBatteryMessage());
            }
        }

        public void StopSpeed()
        {
            m_Rigidbody.velocity = Vector2.zero;
        }

        public void DockingAttempt()
        {
            RaycastHit2D[] raycastHits = Physics2D.RaycastAll(m_DockingSide.transform.position, m_DockingSide.transform.up, 0.6f);
            for (int i = 0; i < raycastHits.Length; i++)
            {
                DockingPoint d = raycastHits[i].collider.GetComponent<DockingPoint>();
                if (d != null && d.IsActive)
                {
                    Debug.Log("Attracco completato");
                    PubSub.PubSub.Publish(new DockingCompleteMessage());
                }
            }
        }

        internal DamageableInfos GetDamageableInfo()
        {
            return new DamageableInfos(m_Damageable.MaxLife, m_Damageable.CurrentLife);
        }

        public void GetDamage(float amount)
        {
            GameManagerES.Instance.UpdateLifeSlider(amount, m_Damageable.MaxLife);
        }
    }
}
