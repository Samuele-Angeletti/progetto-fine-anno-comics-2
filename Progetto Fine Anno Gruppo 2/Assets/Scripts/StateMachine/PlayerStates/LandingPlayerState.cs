using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class LandingPlayerState : State
{
    private PlayerMovementManager m_Owner;
    private Vector2 m_FallingSpeed;
    public LandingPlayerState(PlayerMovementManager owner)
    {
        m_Owner = owner;
    }

    public override void MyOnCollisionEnter2D(Collision2D collision)
    {

    }

    public override void OnEnd()
    {
    }

    public override void OnFixedUpdate()
    {
        m_Owner.InputDirection = new Vector2(m_Owner.InputDirection.x, 0);
        m_Owner.Movement();
        Land();
    }

    public override void OnStart()
    {
        Debug.Log("STATO: LANDING");
        m_Owner.Skeleton.loop = false;
        m_Owner.Skeleton.AnimationName = "SaltoDiscesa";
        m_FallingSpeed = Vector2.zero;
    }

    public override void OnUpdate()
    {
        if (m_Owner.IsGrounded)
           {
            m_Owner.Rigidbody.velocity = new Vector2(m_FallingSpeed.x, 0);
            m_Owner.InputDirection = new Vector2(m_Owner.InputDirection.x, 0);
            m_Owner.StateMachine.SetState(EPlayerState.Landed);
        }
        
    }

    private void Land()
    {
        float castXSpeed = m_Owner.Rigidbody.velocity.x;
        float castYSpeed = Vector2.up.y * m_Owner.GravityScale * Time.fixedDeltaTime;
        m_FallingSpeed += new Vector2(0, castYSpeed);
        m_FallingSpeed = new Vector2(castXSpeed, Mathf.Clamp(m_FallingSpeed.y, m_Owner.GravityScale, 0));

        m_Owner.Rigidbody.velocity = m_FallingSpeed;
    }


}