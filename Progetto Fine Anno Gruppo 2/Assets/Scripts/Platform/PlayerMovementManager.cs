using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PubSub;
using Commons;
using System;

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
        [HideInInspector]
        public Animator PlayerAnimator;
        [HideInInspector]
        public SpriteRenderer SpriteRenderer;
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

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            PlayerCollider = GetComponent<Collider2D>();
            PlayerAnimator = GetComponent<Animator>();
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
            if(collision.GetComponent<Wind>() != null)
            {
                StateMachine.SetState(EPlayerState.Flying);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Wind>() != null)
            {
                StateMachine.SetState(EPlayerState.Walking);
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

            if (Rigidbody.velocity.x > 0.001f && SpriteRenderer.flipX)
            {
                FlipSpriteOnX(true);
            }
            if (Rigidbody.velocity.x < -0.001f && !SpriteRenderer.flipX)
            {
                FlipSpriteOnX(false);
            }

            Rigidbody.velocity = InputDirection * MaxSpeed * Time.fixedDeltaTime;

        }

        internal void SetSpriteXPos(float rightXPos)
        {
            SpriteRenderer.transform.localPosition = new Vector3(rightXPos, 0, 0);
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
            SpriteRenderer.flipX = !flipped;
        }

        public void FlipSpriteOnY(bool flipped)
        {
            SpriteRenderer.flipY = !flipped;
        }

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
                m_Interacter.GetInteractable()?.Interact();
            }
        }

        public void RotatePlayer(int degrees)
        {
            SpriteRenderer.gameObject.transform.eulerAngles = new Vector3(0, 0, degrees);
        }

    }

}