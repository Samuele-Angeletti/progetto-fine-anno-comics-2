using Cinemachine;
using Commons;
using MainGame;
using PubSub;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace ArchimedesMiniGame
{
    public class Module : Controllable, IDamageable, ISubscriber
    {
        [Header("References")]
        [SerializeField] PolygonCollider2D m_ExternalCollider;
        [SerializeField] SpriteRenderer m_ExternalSprite;
        [SerializeField] GameObject m_MapParent;
        [SerializeField] Transform m_DockingPivot;
        [SerializeField] Transform m_DestinationDockingPoint;
        [SerializeField] CinemachineVirtualCamera m_MyCameraOnPlayer; 
        [Space(10)]
        [Header("Main settings")]
        [SerializeField] float m_Acceleration;
        [SerializeField] float m_MaxSpeed;
        [SerializeField] float m_RotationSpeed;
        [SerializeField] float m_MaxSpeedForDocking = 5f;
        [SerializeField] GameObject m_DockingSide;
        [SerializeField] LayerMask m_DockingMask;
        [SerializeField] float m_DockingSpeed = 1;
        [Space(10)]
        [Header("Battery Settings")]
        [SerializeField] float m_MaxBattery;
        [SerializeField] float m_UseSpeed;
        [Space(10)]
        [SerializeField] Transform m_BurstPivot;
        [SerializeField] ParticleSystem m_BurstVFXPrefab;
        [Header("Docking Event")]
        [SerializeField] UnityEvent m_OnDocking;

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
        private bool m_CameraFocused;
        private Vector3 m_StoppingDistanceModucleFocused = new Vector3(10, 10);
        private Vector3 m_RotationDestination;
        private SavableEntity m_SavableEntity;
        public SavableEntity SavableEntity => m_SavableEntity;
        public Transform DockingPoint => m_DockingSide.transform;
        private SpriteTransition m_SpriteTransition;
        private bool m_EngineOn;
        private ParticleSystem m_SpawnedEffect;
        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_CurrentBattery = m_MaxBattery;
            m_Damageable = GetComponent<Damageable>();
            m_Rigidbody.freezeRotation = true;
            m_MaxSpeedVector = new Vector2(m_MaxSpeed, m_MaxSpeed);
            m_SavableEntity = GetComponent<SavableEntity>();
            m_SpriteTransition = GetComponent<SpriteTransition>();
        }

        private void Start()
        {
            GameManagerES.Instance.UpdateBatterySlider(m_CurrentBattery, m_MaxBattery);
            GameManagerES.Instance.UpdateLifeSlider(m_Damageable.CurrentLife, m_Damageable.MaxLife);

            PubSub.PubSub.Subscribe(this, typeof(SaveMessage));
            PubSub.PubSub.Subscribe(this, typeof(LoadMessage));
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));

            m_MapParent.SetActive(false);
            m_ExternalSprite.gameObject.SetActive(true);
            m_Rigidbody.bodyType = RigidbodyType2D.Static;
        }

        private void FixedUpdate()
        {
            if (!m_Docking && !m_Docked && m_CurrentBattery > 0 && m_EngineOn)
            {
                if (m_Direction.magnitude != 0)
                    m_CurrentAcceleration += new Vector2(MathF.Abs(m_Direction.normalized.x), MathF.Abs(m_Direction.normalized.y)) * m_Acceleration * Time.fixedDeltaTime;
                else
                    m_CurrentAcceleration = Vector2.zero;

                UpdateMovement();
                UpdateRotation();
            }
        }

        private void UpdateRotation()
        {
            m_Rigidbody.rotation += m_RotationDirection.x * Time.fixedDeltaTime * m_RotationSpeed;
        }

        private void UpdateMovement()
        {
            m_Rigidbody.velocity += -m_Direction.normalized * m_Acceleration * Time.fixedDeltaTime;
        }

        private void Update()
        {
            if (!m_Docked && m_CurrentBattery > 0 && m_EngineOn)
            {
                if (!m_Docking)
                {
                    DockingButtonChangeCheck();

                    CameraChangeCheck();

                    if (m_Direction != Vector2.zero || m_RotationDirection != Vector2.zero)
                    {
                        UseBattery();
                        UpdateBurstPivot();
                        SpawnVFX();
                    }
                    else
                    {
                        if (m_BurstPivot.gameObject.activeSelf)
                        {
                            if (m_SpawnedEffect != null)
                            {
                                m_SpawnedEffect.transform.parent = null;
                                m_SpawnedEffect.Stop();
                            }
                            m_BurstPivot.gameObject.SetActive(false);
                        }
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
                else if (m_Docking)
                {
                    Docking();
                }

                if (m_CurrentBattery <= 0)
                {
                    Stop();
                }

            }

        }

        private void DockingButtonChangeCheck()
        {
            if (ForwardCheckOfWall(Vector2.up, 0.6f, m_DockingSide.transform.position, m_DockingMask) && m_Rigidbody.velocity.magnitude <= m_MaxSpeedForDocking)
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

        private void CameraChangeCheck()
        {
            if (ForwardCheckOfWall(Vector2.up, 1f, m_DockingSide.transform.position, m_DockingMask, m_StoppingDistanceModucleFocused))
            {
                GameManager.Instance.SetActiveCamera(ECameras.ModuleFocused);
                m_CameraFocused = true;
            }
            else if (m_CameraFocused)
            {
                m_CameraFocused = false;
                GameManager.Instance.SetActiveCamera(ECameras.Module);
            }
        }

        private void UpdateBurstPivot()
        {
            if (!m_BurstPivot.gameObject.activeSelf) m_BurstPivot.gameObject.SetActive(true);

            if (m_RotationDirection.x > 0)
            {
                m_BurstPivot.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 165);
            }
            else if (m_RotationDirection.x < 0)
            {
                m_BurstPivot.eulerAngles = transform.eulerAngles + new Vector3(0, 0, -165);
            }
            else if(m_Direction.x > 0)
            {
                m_BurstPivot.eulerAngles = transform.eulerAngles + new Vector3(0, 45, 180);
            }
            else
            {
                m_BurstPivot.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 180);

            }


        }

        private void LateUpdate()
        {
            if (m_EngineOn)
            {
                GameManagerES.Instance.UpdateSpeed(m_Rigidbody.velocity.magnitude, m_MaxSpeedVector.magnitude);
                GameManagerES.Instance.UpdateAcceleration(m_CurrentAcceleration.magnitude, m_MaxSpeed);
            }
        }

        private void Docking()
        {
            Vector3 cVertex;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, m_RotationDestination, Time.deltaTime * m_DockingSpeed);
            if(m_RotationDestination.z == 180 || m_RotationDestination.z == 0)
            {
                cVertex = new Vector3(m_DockingPoint.DockingPivot.position.x, transform.position.y);
            }
            else
            {
                cVertex = new Vector3(transform.position.x, m_DockingPoint.DockingPivot.position.y);
            }
            transform.position = Vector3.Lerp(transform.position, cVertex, Time.deltaTime * m_DockingSpeed);

            if (Vector3.Distance(transform.eulerAngles, m_RotationDestination) < 0.1f)
            {
                if (Vector3.Distance(transform.position, cVertex) < 0.1f)
                {
                    Vector3 distanceBefore = m_DockingPivot.position - transform.position;
                    m_DockingPivot.position = Vector3.Lerp(m_DockingPivot.position, m_DockingPoint.DockingPivot.position, Time.deltaTime * m_DockingSpeed);
                    transform.position = transform.position + ((m_DockingPivot.position - transform.position) - distanceBefore);

                    if(Vector3.Distance(m_DockingPivot.position, m_DockingPoint.DockingPivot.position) <0.1f)
                        DockingComplete();
                }
            }
        }

        public void TryStartEngine()
        {
            if (!m_Docked && m_CurrentBattery > 0 && m_Damageable.CurrentLife > 0)
            {
                StartEngine();
            }
            else if (m_CurrentBattery <= 0 || m_Damageable.CurrentLife <= 0)
            {
                GameManagerES.Instance.CheckModuleOnStartEngine(this);
                if (!m_Docked && m_CurrentBattery > 0 && m_Damageable.CurrentLife > 0)
                {
                    StartEngine();
                }
            }
        }

        private void StartEngine()
        {
            PubSub.PubSub.Publish(new StartEngineModuleMessage(this, m_DestinationDockingPoint));
            m_Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            m_EngineOn = true;

            m_MapParent.SetActive(false);
            m_Rigidbody.freezeRotation = false;

            GameManagerES.Instance.UpdateLifeSlider(m_Damageable.CurrentLife, m_Damageable.MaxLife);
            GameManagerES.Instance.UpdateBatterySlider(m_CurrentBattery, m_MaxBattery);
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

            if (m_CurrentBattery <= 0)
            {
                m_CurrentBattery = 0;
                GameManagerES.Instance.UpdateBatterySlider(m_CurrentBattery, m_MaxBattery);
                PubSub.PubSub.Publish(new NoBatteryMessage());
            }
        }

        public void Stop()
        {
            m_Rigidbody.freezeRotation = true;
            m_Direction = Vector2.zero;
            m_RotationDirection = Vector2.zero;
            m_Rigidbody.rotation = transform.eulerAngles.z;
            m_Rigidbody.velocity = Vector2.zero;
        }

        public void DockingAttempt()
        {
            if (m_Rigidbody.velocity.magnitude > m_MaxSpeedForDocking)
            {
                GameManagerES.Instance.SendLogMessage("VELOCITA' CORRENTE TROPPO ELEVATA, RALLENTARE E RIPROVARE", 2f);
                return;
            }
            RaycastHit2D[] raycastHits = Physics2D.RaycastAll(m_DockingSide.transform.position, m_DockingSide.transform.up, 0.6f);
            for (int i = 0; i < raycastHits.Length; i++)
            {
                DockingPoint d = raycastHits[i].collider.GetComponent<DockingPoint>();
                if (d != null && d.IsActive)
                {
                    GameManagerES.Instance.SendLogMessage("AGGANGIO ESEGUITO, ATTENDERE", 2f);
                    m_Rigidbody.freezeRotation = true;
                    m_Rigidbody.freezeRotation = false;
                    m_Rigidbody.velocity = Vector3.zero;
                    m_Docking = true;
                    m_DockingPoint = d;
                    SwitcherSystem.SwitchDirection(m_DockingPoint.Orientation,
                        () => m_RotationDestination = new Vector3(0, 0, 180),
                        () => m_RotationDestination = new Vector3(0, 0, 0),
                        () => m_RotationDestination = new Vector3(0, 0, 270),
                        () => m_RotationDestination = new Vector3(0, 0, 90));
                    GameManagerES.Instance.ActiveDockingAttemptButton(false);
                }
                else if (d == null || !d.IsActive)
                {
                    GameManagerES.Instance.SendLogMessage("ATTRACCO FALLITO. PUNTO D'ATTRACCO NON ATTIVO", 2f);
                }
                else
                {
                    GameManagerES.Instance.SendLogMessage("ATTRACCO FALLITO. DISTANZA MINIMA NON RAGGIUNTA", 2f);
                }
            }
        }

        private void DockingComplete()
        {
            m_Docking = false;
            transform.eulerAngles = m_RotationDestination;
            Stop();
            m_Docked = true;
            m_DockingPivot.transform.parent = gameObject.transform;
            m_EngineOn = false;


            if(GetComponent<Capsula>() != null) SetCameraToGameManager();

            Damager d = gameObject.AddComponent<Damager>();
            Damager archimedesDamager = GameManager.Instance.Archimedes.GetComponent<Damager>();
            d.Initialize(archimedesDamager.DamageAmount, archimedesDamager.LayerMask);
            gameObject.transform.parent = GameManager.Instance.Archimedes.transform;
            gameObject.layer = GameManager.Instance.Archimedes.gameObject.layer;
            gameObject.isStatic = true;
            m_Rigidbody.bodyType = RigidbodyType2D.Static;

            m_OnDocking.Invoke();
            PubSub.PubSub.Publish(new DockingCompleteMessage(this));
        }

        public override void Interact()
        {
            if (!m_Docking) DockingAttempt();
        }

        internal SavableInfos GetSavableInfos()
        {
            return new SavableInfos(m_Damageable.MaxLife, m_Damageable.CurrentLife, m_MaxBattery, m_CurrentBattery, transform.position);
        }

        internal void SetInitialParameters(SavableInfos infos)
        {
            m_Damageable.SetMaxLife(infos.MaxLife);
            m_Damageable.SetInitialLife(infos.CurrentLife);
            m_CurrentBattery = infos.CurrentBattery;
            transform.position = new Vector3(infos.xPos, infos.yPos, infos.zPos);
        }

        public void GetDamage(float amount)
        {
            GameManagerES.Instance.UpdateLifeSlider(amount, m_Damageable.MaxLife);
            if (m_Damageable.CurrentLife <= 0) Stop();
        }

        public void OnPublish(IMessage message)
        {
            if (message is SaveMessage)
            {
                string id = GetComponent<SavableEntity>().Id;
                SaveAndLoadSystem.StoreSaveData(id, GetSavableInfos());
            }
            else if (message is LoadMessage)
            {
                LoadMessage loadMessage = (LoadMessage)message;
                if (loadMessage.Database.ContainsKey(m_SavableEntity.Id))
                {

                    SetInitialParameters(loadMessage.Database[m_SavableEntity.Id]);

                }
            }
            else if (message is StartEngineModuleMessage)
            {
                m_SpriteTransition.ActiveSpriteTransparencyTransition(m_ExternalSprite, EDirection.Up);
                m_ExternalCollider.enabled = true;
                m_MapParent.SetActive(false);
            }
            else if (message is DockingCompleteMessage || message is NoBatteryMessage || message is ModuleDestroyedMessage)
            {
                m_SpriteTransition.ActiveSpriteTransparencyTransition(m_ExternalSprite, EDirection.Down);
                m_ExternalCollider.enabled = false;
                m_MapParent.SetActive(true);
            }
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(SaveMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(LoadMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }

        public void SetCameraToGameManager()
        {
            GameManager.Instance.SetCameraOnPlayer(m_MyCameraOnPlayer);
        }

        public void SpawnVFX()
        {
            if (m_SpawnedEffect == null)
            {
                m_SpawnedEffect = Instantiate(m_BurstVFXPrefab, m_BurstPivot.transform);
                Destroy(m_SpawnedEffect.gameObject, 5f);
            }
        }
    }
}
