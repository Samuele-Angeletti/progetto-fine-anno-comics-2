using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
public class JumpingPlayerState : State
{
    private PlayerMovementManager m_Owner;
    private float m_TimePassed = 0;
    private float m_JumpHeigth;
    public JumpingPlayerState(PlayerMovementManager owner)
    {
        m_Owner = owner;
    }

    public override void MyOnCollisionEnter2D(Collision2D collision)
    {

    }

    public override void OnEnd()
    {
        m_TimePassed = 0;
        m_Owner.PlayerAnimator?.SetBool("Jumping", false);
    }

    public override void OnFixedUpdate()
    {
        m_Owner.InputDirection = new Vector2(m_Owner.InputDirection.x, 0);
        m_Owner.Movement();
        Jump();
    }

    public override void OnStart()
    {
        Debug.Log("STATO: JUMPING");
        m_TimePassed = 0;
        m_Owner.PlayerAnimator?.SetBool("Jumping", true);
        m_JumpHeigth = m_Owner.JumpHeight;
    }

    public override void OnUpdate()
    {
        if (!m_Owner.IsJumping && !m_Owner.IsGrounded)
        {
            m_Owner.StateMachine.SetState(EPlayerState.Landing);
        }

        m_TimePassed += Time.deltaTime;
        if (m_TimePassed >= m_Owner.TimerJumpButtonIsPressed)
        {
            m_TimePassed = 0;
            m_Owner.IsJumping = false;
        }

        if (!m_Owner.ForwardCheck(Vector3.up, 1f))
        {
            m_Owner.StateMachine.SetState(EPlayerState.Landing);
        }
    }

    private void Jump()
    {
        m_Owner.Rigidbody.velocity = new Vector2(m_Owner.Rigidbody.velocity.x, Vector2.up.y * m_JumpHeigth * Time.fixedDeltaTime);
        m_JumpHeigth += Time.fixedDeltaTime * m_Owner.JumpDecelerator;
    }

    //private void Movement()
    //{



    //    if (Mathf.Abs(m_Owner.Rigidbody.velocity.x) > m_Owner.MaxSpeed)
    //    {
    //        m_Owner.Rigidbody.velocity = new Vector2(Mathf.Sign(m_Owner.Rigidbody.velocity.x) * m_Owner.MaxSpeed, m_Owner.Rigidbody.velocity.y);
    //    }

    //    if (m_Owner.Direction.x != 0)
    //    {

    //        if (m_Owner.Rigidbody.velocity.x > 0)
    //        {
    //            m_Owner.FlipSpriteOnX(true);
    //        }
    //        if (m_Owner.Rigidbody.velocity.x < -0.1)
    //        {
    //            m_Owner.FlipSpriteOnX(false);
    //        }
    //    }
    //    m_Owner.Rigidbody.velocity = m_Owner.Direction * m_Owner.MaxSpeed * Time.fixedDeltaTime;
    //}

}