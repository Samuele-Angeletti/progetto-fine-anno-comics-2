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
    private bool m_Somersaulting;
    private Vector2 m_Adjuster;
    public ZeroGPlayerState(PlayerMovementManager owner)
    {
        m_Owner = owner;
    }

    public override void MyOnCollisionEnter2D(Collision2D collision)
    {
    }

    public override void OnEnd()
    {
        SetAdjuster(Vector2.zero);
        if(!m_Somersaulting)
            m_Owner.RotatePlayer(0, m_Adjuster);
        m_CurrentDirection = Vector2.zero;
    }

    public override void OnFixedUpdate()
    {
        Movement();
    }

    public override void OnStart()
    {
        Debug.Log("STATO: ZERO G");
        m_Owner.Skeleton.loop = true;
        m_Owner.Skeleton.AnimationName = "Idol";
        m_IsMoving = false;
        m_Somersaulting = false;
        m_Adjuster = Vector2.zero;
    }

    public override void OnUpdate()
    {
        if (m_Owner.InputDirection.x != 0)
        {
            if (m_Owner.ForwardCheckOfWall(m_Owner.InputDirection, 0.5f, m_Owner.Skeleton.transform.position + (Vector3.up / 2)))
            {
                Move();
            }
        }
        else
        {
            if (m_Owner.ForwardCheckOfWall(m_Owner.InputDirection, 1f))
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
                FlipSprite(270, 90, 0, 180, false);
                m_Owner.Skeleton.loop = true;
                m_Owner.Skeleton.AnimationName = "GravitàApice";
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
                    else
                    {
                        if (!m_Owner.ForwardCheckOfWall(m_CurrentDirection, 0.5f)) // questo non detecta le piattaforme perchè sono troppo in alto, da rivedere
                        {
                            StopMovement();
                        }
                    }
                }
            }
        }
    }

    private void StopMovement()
    {
        FlipSprite(90, 270, 180, 0, true);
        m_CurrentDirection = Vector3.zero;
        m_IsMoving = false;
        m_Somersaulting = true;
        m_Owner.StateMachine.SetState(EPlayerState.Somersault);
    }

    private void Movement()
    {
        m_Owner.Rigidbody.velocity = m_Owner.SpeedInZeroGravity * Time.fixedDeltaTime * m_CurrentDirection;
    }

    private void FlipSprite(int right, int left, int up, int down, bool stopping)
    {
        if(m_CurrentDirection.x > 0)
        {
            if (stopping)
                SetAdjuster(new Vector2(1, 0));
            else
                SetAdjuster(new Vector2(0, 0.5f));

            m_Owner.RotatePlayer(right, m_Adjuster);
        }
        else if(m_CurrentDirection.y > 0)
        {
            if (stopping)
                SetAdjuster(new Vector2(0, 1));

            m_Owner.RotatePlayer(up, m_Adjuster);
        }
        else if(m_CurrentDirection.x < 0)
        {
            if (stopping)
                SetAdjuster(new Vector2(-1, 0));
            else
                SetAdjuster(new Vector2(0, 0.5f));

            m_Owner.RotatePlayer(left, m_Adjuster);
        }
        else if(m_CurrentDirection.y < 0)
        {
            if (stopping)
                SetAdjuster(new Vector2(0, -1));

            m_Owner.RotatePlayer(down, m_Adjuster);
        }
    }

    private void SetAdjuster(Vector2 newAdjuster)
    {
        if (m_Adjuster.y != 0)
            newAdjuster = new Vector2(newAdjuster.x, -m_Adjuster.y);

        m_Adjuster = newAdjuster;
    }
}
