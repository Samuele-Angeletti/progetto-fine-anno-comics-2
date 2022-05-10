using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class ZeroGPlayerState : State
{
    private PlayerMovementManager m_Owner;

    private Vector2 m_CurrentDirection;
    private bool m_IsMoving;

    public ZeroGPlayerState(PlayerMovementManager owner)
    {
        m_Owner = owner;
    }

    public override void MyOnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public override void OnEnd()
    {
        m_Owner.PlayerAnimator?.SetBool("IdleZeroG", false);
        m_Owner.RotatePlayer(0);
    }

    public override void OnFixedUpdate()
    {
        Movement();
    }

    public override void OnStart()
    {
        Debug.Log("STATO: ZERO G");
        m_Owner.PlayerAnimator?.SetBool("IdleZeroG", true);
        m_IsMoving = false;
    }

    public override void OnUpdate()
    {
        m_IsMoving = m_Owner.Rigidbody.velocity != Vector2.zero;

        if (!m_IsMoving)
        {
            if (m_Owner.Direction.magnitude != 0)
            {
                m_CurrentDirection = m_Owner.Direction;
                m_Owner.PlayerAnimator?.SetBool("JumpingZeroG", true);
            }
        }
    }

    private void Movement()
    {
        if (m_IsMoving) return;

        
        m_Owner.Rigidbody.velocity = m_Owner.Direction * m_Owner.SpeedInZeroGravity * Time.fixedDeltaTime;

        if (m_CurrentDirection.magnitude != 0)
        {
            if (m_Owner.Rigidbody.velocity == Vector2.zero)
            {
                FlipSprite();
                m_CurrentDirection = Vector3.zero;
            }
        }
    }

    private void FlipSprite()
    {
        if(m_CurrentDirection.x > 0)
        {
            //m_Owner.FlipSpriteOnX(false);
            m_Owner.RotatePlayer(90);
        }
        else if(m_CurrentDirection.y > 0)
        {
            //m_Owner.FlipSpriteOnY(false);
            m_Owner.RotatePlayer(180);
        }
        else if(m_CurrentDirection.x < 0)
        {
            //m_Owner.FlipSpriteOnX(true);
            m_Owner.RotatePlayer(270);
        }
        else if(m_CurrentDirection.y < 0)
        {
            //m_Owner.FlipSpriteOnY(true);
            m_Owner.RotatePlayer(0);
        }

        m_Owner.PlayerAnimator?.SetBool("JumpingZeroG", false);
    }
}
