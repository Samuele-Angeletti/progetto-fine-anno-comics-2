using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace MainGame
{
    public class PlayerPlatformMovement : State
    {
        private GenericStateMachine<EPlayerState> m_playerStateMachine;
        private InputControls inputActions;
        private Rigidbody2D m_rigidbody;
        private Collider2D m_playerCollider;
        private Animator m_playerAnimator;
        private SpriteRenderer m_spriteRenderer;
        private float m_timerJump;
        private float m_coyoteTime = 0.15f;
        private float m_coyoteTimeCounter;



        private int m_speed;
        private float m_jumpHeight;
        private float m_movementVelocity;
        private float m_timerJumpButtonIsPressed;
        private LayerMask m_groundMask;


        public bool isGrounded, isJumping;
        private Vector2 m_direction;

        public PlayerPlatformMovement(GenericStateMachine<EPlayerState> stateMachine, PlayerMovementManager manager, Rigidbody2D playerRb2D, Collider2D playerCollider, Animator playerAnimator,
            SpriteRenderer playerRenderer, int speed, float jumpHeight, float movementVelocity, float timerButtonIsPressed, LayerMask groundMask)
        {
            inputActions = new InputControls();
            m_playerStateMachine = stateMachine;
            m_rigidbody = playerRb2D;
            m_playerCollider = playerCollider;
            m_playerAnimator = playerAnimator;
            m_spriteRenderer = playerRenderer;
            m_speed = speed;
            m_jumpHeight = jumpHeight;
            m_movementVelocity = movementVelocity;
            m_timerJumpButtonIsPressed = timerButtonIsPressed;
            m_groundMask = groundMask;
        }

        public override void OnStart()
        {
            inputActions.Player.Enable();
        }

        public override void OnUpdate()
        {
            Jump();
            CoyoteTimeAndGroundCheck();
        }

        public override void OnEnd()
        {
            inputActions.Player.Disable();
        }

        public override void MyOnCollisionEnter2D(Collision2D collision)
        {

        }

        public override void OnFixedUpdate()
        {
            Movement();
        }


        private bool CheckIfGrounded()
        {
            Color rayColor;
            RaycastHit2D hit = Physics2D.BoxCast(m_playerCollider.bounds.center, m_playerCollider.bounds.size, 0, Vector2.down, 0.1f, m_groundMask);
            if (hit.collider != null)
            {
                rayColor = Color.green;
            }
            else
                rayColor = Color.red;
            isGrounded = hit.collider != null;
            Debug.DrawRay(m_playerCollider.bounds.center, Vector2.down * (m_playerCollider.bounds.extents.y + 0.1f), rayColor);
            return isGrounded;

        }
        private void Jump()
        {
            //m_playerAnimator.SetFloat("JumpVelocity", m_rigidbody.velocity.y);

            if (inputActions.Player.Jump.ReadValue<float>() == 1 && m_coyoteTimeCounter > 0)
            {
                isJumping = true;
                m_timerJump = m_timerJumpButtonIsPressed;
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, Vector2.up.y * m_jumpHeight);
            }
            if (inputActions.Player.Jump.IsPressed() && isJumping == true)
            {

                if (m_timerJump > 0)
                {
                    m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, Vector2.up.y * m_jumpHeight);
                    m_timerJump -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }

            }
            if (inputActions.Player.Jump.ReadValue<float>() == 0)
                isJumping = false;




        }
        private void CoyoteTimeAndGroundCheck()
        {
            if (CheckIfGrounded())
            {
                m_coyoteTimeCounter = m_coyoteTime;
            }
            else
            {
                m_coyoteTimeCounter -= Time.deltaTime;
            }
        }


        private void Movement()
        {
            m_direction = inputActions.Player.MovementArrows.ReadValue<Vector2>();
            if (Mathf.Abs(m_rigidbody.velocity.x) > m_movementVelocity)
            {
                m_rigidbody.velocity = new Vector2(Mathf.Sign(m_rigidbody.velocity.x) * m_movementVelocity, m_rigidbody.velocity.y);
            }

            if (inputActions.Player.MovementArrows.ReadValue<Vector2>().x != 0)
            {

                if (m_rigidbody.velocity.x > 0)
                {
                    FlipSprite(true);
                }
                if (m_rigidbody.velocity.x < -0.1)
                {
                    FlipSprite(false);
                }
                m_playerAnimator?.SetBool("IsMoving", true);
            }
            if (inputActions.Player.MovementArrows.ReadValue<Vector2>().x == 0)
            {
                m_playerAnimator?.SetBool("IsMoving", false);

            }
            m_rigidbody.velocity += m_direction * m_speed * Time.fixedDeltaTime;
        }

        private void FlipSprite(bool v)
        {
            m_spriteRenderer.flipX = !v;
        }


    }
}
