using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MainGame
{
    public class PlayerMovementManager : MonoBehaviour
    {
        private GenericStateMachine<EPlayerState> m_stateMachine;
        private InputControls inputActions;
        private Rigidbody2D m_rigidbody;
        private Collider2D m_playerCollider;
        private Animator m_playerAnimator;
        private SpriteRenderer m_spriteRenderer;
        private int m_speed = 40;

        [Header("Per Stato Platform")]
        [Range(0, 30)]
        public float jumpHeight;
        [Range(0, 30)]
        public float movementVelocity;
        [Range(0f, 1f)]
        public float timerJumpButtonIsPressed;
        public LayerMask groundMask;
        public bool isGrounded, isJumping;


        [Header("Per stato zero gravity")]
        public float speedInZeroGravity;

        private Vector2 m_direction;

        private void OnEnable()
        {
            inputActions?.Player.Enable();

        }
        private void OnDisable()
        {
            inputActions?.Player.Disable();
        }
        private void Awake()
        {
            inputActions = new InputControls();
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_playerCollider = GetComponent<Collider2D>();
            m_playerAnimator = GetComponent<Animator>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_stateMachine = new GenericStateMachine<EPlayerState>();

        }
        // Start is called before the first frame update
        void Start()
        {
            m_stateMachine.RegisterState(EPlayerState.PlatformMovement, new PlayerPlatformMovement(m_stateMachine, this, m_rigidbody, m_playerCollider, m_playerAnimator,
                m_spriteRenderer, m_speed, jumpHeight, movementVelocity, timerJumpButtonIsPressed, groundMask));
            m_stateMachine.RegisterState(EPlayerState.ZeroGravityMovement, new PlayerZeroGravityMovement(m_stateMachine, m_rigidbody, m_playerCollider,
                m_playerAnimator, m_spriteRenderer, speedInZeroGravity));
            m_stateMachine.SetState(EPlayerState.PlatformMovement);
        }

        // Update is called once per frame
        void Update()
        {
            //m_stateMachine.OnUpdate();                           
            //if (inputActions.Player.ChangeState.IsPressed())                          // INPUT SYSTEM TO FIX
            //{
            //    m_stateMachine.SetState(EPlayerState.ZeroGravityMovement);
            //}
        }
        private void FixedUpdate()
        {
            m_stateMachine.OnFixedUpdate();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            m_stateMachine.MyOnCollisionEnter2D(collision);
        }
    }

}
