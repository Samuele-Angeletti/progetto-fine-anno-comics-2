using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MainGame
{
    public class PlayerMovementManager : Controllable
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

        [Header("Per Stato Platform")]
        [Range(0, 30)]
        public float JumpHeight;
        [Range(0, 30)]
        public float MovementVelocity;
        [Range(0f, 1f)]
        public float TimerJumpButtonIsPressed;
        public LayerMask GroundMask;
        public bool IsGrounded, IsJumping;


        [Header("Per stato zero gravity")]
        public float m_SpeedInZeroGravity;

        public Vector2 Direction;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            PlayerCollider = GetComponent<Collider2D>();
            PlayerAnimator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            StateMachine = new GenericStateMachine<EPlayerState>();

        }

        void Start()
        {
            //m_stateMachine.RegisterState(EPlayerState.PlatformMovement, new PlayerPlatformMovement(m_stateMachine, this, m_rigidbody, m_playerCollider, m_playerAnimator,
            //    m_spriteRenderer, m_speed, jumpHeight, movementVelocity, timerJumpButtonIsPressed, groundMask));
            //m_stateMachine.RegisterState(EPlayerState.ZeroGravityMovement, new PlayerZeroGravityMovement(m_stateMachine, m_rigidbody, m_playerCollider,
            //    m_playerAnimator, m_spriteRenderer, speedInZeroGravity));
            //m_stateMachine.SetState(EPlayerState.PlatformMovement);
            StateMachine.RegisterState(EPlayerState.Walking, new WalkingPlayerState(this));

            StateMachine.SetState(EPlayerState.Walking);
        }


        void Update()
        {
            StateMachine.OnUpdate();
            //if (inputActions.Player.ChangeState.IsPressed())
            //{
            //    m_stateMachine.SetState(EPlayerState.ZeroGravityMovement);
            //}
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

    }

}
