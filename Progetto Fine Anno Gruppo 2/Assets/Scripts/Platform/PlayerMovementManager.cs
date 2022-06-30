using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PubSub;
using Commons;
using System;
using Spine;
using Spine.Unity;

namespace MainGame
{
    public class PlayerMovementManager : Controllable, ISubscriber
    {
        [HideInInspector]
        public GenericStateMachine<EPlayerState> StateMachine;
        [HideInInspector]
        public Rigidbody2D Rigidbody;
        [HideInInspector]
        public Collider2D PlayerCollider;
        [SerializeField]
        public SkeletonAnimation Skeleton;
        [SerializeField] 
        public GameObject GraphicsPivot;
        [HideInInspector]
        public bool IsGrounded, IsJumping;

        [Header("Movement Settings")]
        public float InertiaTime;
        public float InertiaDecelerator;
        public float MaxSpeed;

        [Header("Jump Settings")]
        public float JumpHeight;
        public float JumpDecelerator;
        public float TimerJumpButtonIsPressed;
        public float GravityScale;
        public LayerMask GroundMask;

        [Header("Zero Gravity Settings")]
        public float SpeedInZeroGravity;

        [Header("Interaction Settings")]
        [SerializeField] public float InteractionAnimationSpeed;

        [Header("Debug Infos")]
        public Vector2 InputDirection;

        private Interacter m_Interacter;
        private Vector3 m_NextCheckPoint;
        private Damageable m_Damageable;

        [HideInInspector]
        public EDirection CurrentDirection;
        public Vector3 NextCheckpoint => m_NextCheckPoint;
        public Interacter Interacter => m_Interacter;
        public bool PassingFromZeroG { get; set; }

        public bool Flipped { get; private set; } = false;
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            PlayerCollider = GetComponent<Collider2D>();
            StateMachine = new GenericStateMachine<EPlayerState>();
            m_Interacter = GetComponent<Interacter>();
            m_NextCheckPoint = transform.position;
            m_Damageable = GetComponent<Damageable>();
        }

        void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(ZeroGMessage));
            PubSub.PubSub.Subscribe(this, typeof(CheckPointMessage));
            PubSub.PubSub.Subscribe(this, typeof(PlayerDeathMessage));
            PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));


            StateMachine.RegisterState(EPlayerState.Walking, new WalkingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Jumping, new JumpingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.ZeroG, new ZeroGPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Landing, new LandingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Flying, new FlyingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Somersault, new SomersaultPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Landed, new LandedPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Interacting, new InteractingPlayerState(this));

            StateMachine.SetState(EPlayerState.Walking);
        }

        void Update()
        {

            StateMachine.OnUpdate();
            GroundCheck();
        }

        private void FixedUpdate()
        {
            StateMachine.OnFixedUpdate();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StateMachine.MyOnCollisionEnter2D(collision);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            JumpIncreaser ji = collision.GetComponentInParent<JumpIncreaser>();

            if (collision.GetComponentInParent<Lift>() != null)
            {
                if (StateMachine.CurrentState.GetType() != typeof(FlyingPlayerState))
                {
                    if (StateMachine.CurrentState is ZeroGPlayerState)
                        PubSub.PubSub.Publish(new ZeroGMessage(false));

                    StateMachine.SetState(EPlayerState.Flying);
                }
            }
            else if(ji != null)
            {
                ji.StoreJumpHegiht(JumpHeight);
                JumpHeight = ji.GetNewJumpHeght();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            JumpIncreaser ji = collision.GetComponentInParent<JumpIncreaser>();

            if (collision.GetComponentInParent<Lift>() != null)
            {
                StateMachine.SetState(EPlayerState.Walking);
            }
            else if(ji != null)
            {
                JumpHeight = ji.GetOldJumpHeght();
            }
        }

        private void GroundCheck()
        {
            if (!ForwardCheckOfWall(Vector3.down, 0.01f))
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }

        public void Movement()
        {
           
            if (InputDirection.x > 0.001f && Flipped)
            {
                FlipSpriteOnX(false);
            }
            if (InputDirection.x < -0.001f && !Flipped)
            {
                FlipSpriteOnX(true);
            }

            Rigidbody.velocity = InputDirection * MaxSpeed * Time.fixedDeltaTime;
             

        }

        public override void MoveDirection(Vector2 newDirection)
        {
            InputDirection = newDirection.normalized;
        }

        public override void Jump(bool jumping)
        {
            IsJumping = jumping;
        }

        public void FlipSpriteOnX(bool flipped)
        {
            Vector3 scale = flipped ? Vector3.back : Vector3.forward;
            Skeleton.gameObject.transform.rotation = Quaternion.LookRotation(scale, Vector3.up);
            Flipped = flipped;
        }


        public void OnPublish(IMessage message)
        {
            if (message is ZeroGMessage)
            {
                ZeroGMessage zeroGMessage = (ZeroGMessage)message;
                ContinousMovement = zeroGMessage.Active;
                CurrentDirection = EDirection.Up;
                if (zeroGMessage.Active)
                    StateMachine.SetState(EPlayerState.ZeroG);
                else
                    StateMachine.SetState(EPlayerState.Walking);
            }
            else if(message is CheckPointMessage)
            {
                CheckPointMessage checkPoint = (CheckPointMessage)message;
                m_NextCheckPoint = checkPoint.Position;
            }
            else if(message is PlayerDeathMessage)
            {
                transform.position = m_NextCheckPoint;
                Rigidbody.velocity = Vector2.zero;
                
                if (StateMachine.CurrentState.GetType() == typeof(ZeroGPlayerState))
                {
                    ZeroGPlayerState zeroGPlayerState = (ZeroGPlayerState)StateMachine.GetState(EPlayerState.ZeroG);
                    zeroGPlayerState.StopMovement();
                }
                m_Damageable.SetInitialLife(1);
            }
            else if(message is StartEngineModuleMessage)
            {
                PlayerCollider.enabled = false;
            }
            else if(message is DockingCompleteMessage || message is NoBatteryMessage || message is ModuleDestroyedMessage)
            {
                PlayerCollider.enabled = true;
            }
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(ZeroGMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(CheckPointMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(PlayerDeathMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }

        public override void Interact()
        {
            
            if (Rigidbody.velocity.magnitude == 0 && Interacter.InteractionAvailable && StateMachine.CurrentState.GetType() != typeof(InteractingPlayerState) && StateMachine.CurrentState.GetType() != typeof(SomersaultPlayerState))
            {
                if (StateMachine.CurrentState.GetType() == typeof(ZeroGPlayerState))
                {
                    PassingFromZeroG = true;
                    ZeroGPlayerState zeroGPlayerState = (ZeroGPlayerState)StateMachine.GetState(EPlayerState.ZeroG);
                    zeroGPlayerState.PassedThroughInteraction = true;
                }
                StateMachine.SetState(EPlayerState.Interacting);
            }
        }

        public void RotatePlayer(int degrees, Vector3 adjusterPos)
        {
            GraphicsPivot.transform.eulerAngles = new Vector3(0, 0, degrees);
            GraphicsPivot.transform.localPosition += adjusterPos;
        }

#if UNITY_EDITOR
        public override void DebugZeroG()
        {
            if (StateMachine.CurrentState is ZeroGPlayerState)
                PubSub.PubSub.Publish(new ZeroGMessage(false));
            else
                PubSub.PubSub.Publish(new ZeroGMessage(true));
        }
#endif 
    }

}