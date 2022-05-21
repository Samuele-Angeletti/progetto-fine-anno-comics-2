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

        [Header("Debug Infos")]
        public Vector2 InputDirection;

        private Interacter m_Interacter;
        private Vector3 m_NextCheckPoint;
        private Damageable m_Damageable;

        public Vector3 NextCheckpoint => m_NextCheckPoint;
        private bool m_Flipped = false;
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


            StateMachine.RegisterState(EPlayerState.Walking, new WalkingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Jumping, new JumpingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.ZeroG, new ZeroGPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Landing, new LandingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Flying, new FlyingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Somersault, new SomersaultPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Landed, new LandedPlayerState(this));

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

            if (Rigidbody.velocity.x > 0.001f && m_Flipped)
            {
                FlipSpriteOnX(false);
            }
            if (Rigidbody.velocity.x < -0.001f && !m_Flipped)
            {
                FlipSpriteOnX(true);
            }

            Rigidbody.velocity = InputDirection * MaxSpeed * Time.fixedDeltaTime;

        }

        internal void SetSpriteXPos(float rightXPos)
        {
            Skeleton.transform.localPosition = new Vector3(rightXPos, 0, 0);
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
            m_Flipped = flipped;
        }

        //public void FlipSpriteOnY(bool flipped)
        //{
        //    float scale = flipped ? -1 : 1;
        //    Skeleton.gameObject.transform.localScale = new Vector2(0, scale);
        //}

        public void OnPublish(IMessage message)
        {
            if (message is ZeroGMessage)
            {
                ZeroGMessage zeroGMessage = (ZeroGMessage)message;
                ContinousMovement = zeroGMessage.Active;
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
                PubSub.PubSub.Publish(new ZeroGMessage(false));
                m_Damageable.SetInitialLife(1);
            }
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(ZeroGMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(CheckPointMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(PlayerDeathMessage));
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }

        public override void Interact()
        {
            if (Rigidbody.velocity.magnitude == 0)
            {
                m_Interacter.GetInteractable()?.Interact(m_Interacter);
            }
        }

        public void RotatePlayer(int degrees)
        {
            Skeleton.gameObject.transform.eulerAngles = new Vector3(0, 0, degrees);
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