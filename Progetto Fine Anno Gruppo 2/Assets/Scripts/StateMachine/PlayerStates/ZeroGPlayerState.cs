using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using MicroGame;
using UnityEngine.Tilemaps;
using System;

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
        m_Owner.SetSpriteXPos(0);
        m_CurrentDirection = Vector2.zero;
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
        if (m_Owner.InputDirection.x != 0 )
        {
            if (m_Owner.ForwardCheckOfWall(m_Owner.InputDirection, 0.5f, m_Owner.SpriteRenderer.transform.position))
            {
                Move();
            }
        }
        else
        {
            if(m_Owner.ForwardCheckOfWall(m_Owner.InputDirection, 1f))
            {
                Move();
            }
        }
        

    }

    private void Move()
    {
        if (!m_IsMoving)
        {
            if (m_Owner.InputDirection.magnitude != 0 && m_CurrentDirection.magnitude == 0)
            {
                m_IsMoving = true;
                m_CurrentDirection = m_Owner.InputDirection;
                FlipSprite(270, 90, 0, 180, -0.233f, 0.233f);
                m_Owner.PlayerAnimator?.SetBool("JumpingZeroG", true);
            }
        }
        else
        {
            if (m_CurrentDirection.magnitude != 0)
            {
                if (m_Owner.Rigidbody.velocity == Vector2.zero)
                {
                    if (m_CurrentDirection.y > 0)
                    {
                        if (!m_Owner.ForwardCheckOfWall(m_CurrentDirection, 1f))
                        {
                            StopMovement();
                        }
                    }
                    else if (!m_Owner.ForwardCheckOfWall(m_CurrentDirection, 0.5f))
                    {
                        StopMovement();
                    }
                }
            }
        }
    }

    private void StopMovement()
    {
        FlipSprite(90, 270, 180, 0, 0.233f, -0.233f);
        m_CurrentDirection = Vector3.zero;
        m_IsMoving = false;

        m_Owner.PlayerAnimator?.SetBool("JumpingZeroG", false);
    }

    private void Movement()
    {
        m_Owner.Rigidbody.velocity = m_Owner.SpeedInZeroGravity * Time.fixedDeltaTime * m_CurrentDirection;
    }

    private void FlipSprite(int right, int left, int up, int down, float leftXPos, float rightXPos)
    {
        if(m_CurrentDirection.x > 0)
        {
            m_Owner.RotatePlayer(right);
            m_Owner.SetSpriteXPos(rightXPos);
        }
        else if(m_CurrentDirection.y > 0)
        {
            m_Owner.RotatePlayer(up);
            m_Owner.SetSpriteXPos(0);
        }
        else if(m_CurrentDirection.x < 0)
        {
            m_Owner.RotatePlayer(left);
            m_Owner.SetSpriteXPos(leftXPos);
        }
        else if(m_CurrentDirection.y < 0)
        {
            m_Owner.RotatePlayer(down);
            m_Owner.SetSpriteXPos(0);
        }

    }
}
