using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PubSub;
using Commons;
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
        public float Speed = 40;
        public float InertiaTime;
        public float InertiaDecelerator;
        public float MaxSpeed;

        [Header("Jump Settings")]
        public float JumpHeight;
        public float TimerJumpButtonIsPressed;
        public float GravityScale;
        public LayerMask GroundMask;

        [Header("Zero Gravity Settings")]
        public float SpeedInZeroGravity;

        public Vector2 Direction;

        private Interacter m_Interacter;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            PlayerCollider = GetComponent<Collider2D>();
            PlayerAnimator = GetComponent<Animator>();
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            StateMachine = new GenericStateMachine<EPlayerState>();
            m_Interacter = GetComponent<Interacter>();
        }

        void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(ZeroGMessage));


            StateMachine.RegisterState(EPlayerState.Walking, new WalkingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Jumping, new JumpingPlayerState(this));
            StateMachine.RegisterState(EPlayerState.ZeroG, new ZeroGPlayerState(this));
            StateMachine.RegisterState(EPlayerState.Landing, new LandingPlayerState(this));
           
            StateMachine.SetState(EPlayerState.Walking);
        }


        void Update()
        {
            StateMachine.OnUpdate();
            GroundCheck();
        }

        private void GroundCheck()
        {
            if (!ForwardCheck(Vector3.down, 1.05f))
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }

        private void FixedUpdate()
        {
            StateMachine.OnFixedUpdate();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StateMachine.MyOnCollisionEnter2D(collision);
        }

        public override void MoveDirection(Vector2 newDirection)
        {
            Direction = newDirection.normalized;
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
            if(message is ZeroGMessage)
            {
                ZeroGMessage zeroGMessage = (ZeroGMessage)message;
                if (zeroGMessage.Active)
                    StateMachine.SetState(EPlayerState.ZeroG);
                else
                    StateMachine.SetState(EPlayerState.Walking);
            }
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(ZeroGMessage));
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
            transform.eulerAngles = new Vector3(0, 0, degrees);
        }
    }

}
