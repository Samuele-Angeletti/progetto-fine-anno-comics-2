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
        m_Owner.Skeleton.loop = false;
        m_Owner.Skeleton.AnimationName = "SaltoApice";
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

        if (!m_Owner.ForwardCheckOfWall(Vector3.up, 1f))
        {
            m_Owner.StateMachine.SetState(EPlayerState.Landing);
        }
    }

    private void Jump()
    {
        m_Owner.Rigidbody.velocity = new Vector2(m_Owner.Rigidbody.velocity.x, Vector2.up.y * m_JumpHeigth * Time.fixedDeltaTime);
        m_JumpHeigth += Time.fixedDeltaTime * m_Owner.JumpDecelerator;
    }


}