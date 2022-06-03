using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using MainGame;

namespace ArchimedesMiniGame
{
    public class Module : Controllable, IDamageable
    {
        [Header("References")]
        [SerializeField] BoxCollider2D m_ExternalCollider;
        [SerializeField] SpriteRenderer m_ExternalSprite;
        [SerializeField] GameObject m_MapParent;
        [SerializeField] Transform m_DockingPivot;
        [Space(10)]
        [Header("Main settings")]
        [SerializeField] float m_Acceleration;
        [SerializeField] float m_MaxSpeed;
        [SerializeField] float m_RotationSpeed;
        [SerializeField] GameObject m_DockingSide;
        [SerializeField] LayerMask m_DockingMask;
        [Space(10)]
        [Header("Battery Settings")]
        [SerializeField] float m_MaxBattery;
        [SerializeField] float m_UseSpeed;

        private float m_CurrentBattery;
        private Rigidbody2D m_Rigidbody;
        private Vector2 m_RotationDirection;
        private Damageable m_Damageable;
        private bool m_DockingAttemptAvailable;
        private bool m_Docked;
        private Vector2 m_MaxSpeedVector;
        private Vector2 m_CurrentAcceleration;
        private DockingPoint m_DockingPoint;
        private bool m_Docking;
        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_CurrentBattery = m_MaxBattery;
            m_Damageable = GetComponent<Damageable>();
            m_Rigidbody.freezeRotation = true;
            m_MaxSpeedVector = new Vector2(m_MaxSpeed, m_MaxSpeed);
        }

        private void Start()
        {
            GameManagerES.Instance.UpdateBatterySlider(m_CurrentBattery, m_MaxBattery);
            GameManagerES.Instance.UpdateLifeSlider(m_Damageable.CurrentLife, m_Damageable.MaxLife);
        }

        private void FixedUpdate()
        {
            if (!m_Docking && !m_Docked)
            {
                if (m_Direction.magnitude != 0)
                    m_CurrentAcceleration += new Vector2(MathF.Abs(m_Direction.normalized.x), MathF.Abs(m_Direction.normalized.y)) * m_Acceleration * Time.fixedDeltaTime;
                else
                    m_CurrentAcceleration = Vector2.zero;


                m_Rigidbody.velocity += -m_Direction.normalized * m_Acceleration * Time.fixedDeltaTime;
                m_Rigidbody.rotation += m_RotationDirection.x * Time.fixedDeltaTime * m_RotationSpeed;
            }
        }

        private void Update()
        {
            if (!m_Docked)
            {
                if (!m_Docking)
                {
                    if (m_Direction != Vector2.zero || m_RotationDirection != Vector2.zero)
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

                    if (ForwardCheckOfWall(Vector2.up, 0.6f, m_DockingSide.transform.position, m_DockingMask) && m_Rigidbody.velocity.magnitude < 5f)
                    {
                        m_DockingAttemptAvailable = true;
                        GameManagerES.Instance.ActiveDockingAttemptButton(true);
                    }
                    else if (m_DockingAttemptAvailable)
                    {
                        m_DockingAttemptAvailable = false;
                        GameManagerES.Instance.ActiveDockingAttemptButton(false);

                    }

                }
                else if (m_Docking)
                {
                    m_DockingPivot.parent = null;
                    Docking();
                }


                GameManagerES.Instance.UpdateSpeed(m_Rigidbody.velocity.magnitude, m_MaxSpeedVector.magnitude);
                GameManagerES.Instance.UpdateAcceleration(m_CurrentAcceleration.magnitude, m_MaxSpeed);
            }
        }

        private void Docking()
        {
            Vector3 distanceBefore = m_DockingPivot.position - transform.position;
            m_DockingPivot.position = Vector3.Lerp(m_DockingPivot.position, m_DockingPoint.DockingPivot.position, 1 * Time.deltaTime);
            transform.position = transform.position + ((m_DockingPivot.position - transform.position) - distanceBefore);

            if (Vector3.Distance(m_DockingPivot.position, m_DockingPoint.DockingPivot.position) < 0.1f)
            {
                DockingComplete();
            }
        }

        public void StartEngine()
        {
            if (!m_Docked)
            {
                PubSub.PubSub.Publish(new StartEngineModuleMessage(this));
                Debug.Log($"START ENGINE: {gameObject.name}");
                m_MapParent.SetActive(false);
                m_Rigidbody.freezeRotation = false;
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
                Stop();
                PubSub.PubSub.Publish(new NoBatteryMessage());
            }
        }

        public void Stop()
        {
            m_Rigidbody.velocity = Vector2.zero;

            m_Rigidbody.freezeRotation = true;
        }

        public void DockingAttempt()
        {
            RaycastHit2D[] raycastHits = Physics2D.RaycastAll(m_DockingSide.transform.position, m_DockingSide.transform.up, 0.6f);
            for (int i = 0; i < raycastHits.Length; i++)
            {
                DockingPoint d = raycastHits[i].collider.GetComponent<DockingPoint>();
                if (d != null && d.IsActive)
                {
                    Debug.Log("AGGANCIO ESEGUITO");
                    m_Rigidbody.freezeRotation = true;
                    m_Rigidbody.freezeRotation = false;
                    m_Rigidbody.velocity = Vector3.zero;
                    m_Docking = true;
                    m_DockingPoint = d;
                    SwitcherSystem.SwitchDirection(m_DockingPoint.Orientation,
                        () => transform.eulerAngles = new Vector3(0, 0, 180),
                        () => transform.eulerAngles = new Vector3(0, 0, 0),
                        () => transform.eulerAngles = new Vector3(0, 0, 270),
                        () => transform.eulerAngles = new Vector3(0, 0, 90));
                }
                else if(d == null || !d.IsActive)
                {
                    Debug.Log("ATTRACCO FALLITO. PUNTO D'ATTRACCO NON ATTIVO");
                }
                else
                {
                    Debug.Log("ATTRACCO FALLITO. DISTANZA MINIMA NON RAGGIUNTA");
                }
            }
        }

        private void DockingComplete()
        {
            Debug.Log("ATTRACCO COMPLETATO");
            m_Docking = false;
            m_ExternalCollider.enabled = false;
            m_ExternalSprite.enabled = false;
            m_MapParent.SetActive(true);
            Stop();
            m_Docked = true;
            PubSub.PubSub.Publish(new DockingCompleteMessage());
        }

        public override void Interact()
        {
            DockingAttempt();
        }

        internal DamageableInfos GetDamageableInfo()
        {
            return new DamageableInfos(m_Damageable.MaxLife, m_Damageable.CurrentLife);
        }

        public void GetDamage(float amount)
        {
            GameManagerES.Instance.UpdateLifeSlider(amount, m_Damageable.MaxLife);
            if (m_Damageable.CurrentLife <= 0) Stop();
        }

        
    }
}
