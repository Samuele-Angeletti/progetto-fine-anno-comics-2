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
    public bool PassedThroughInteraction;

    public ZeroGPlayerState(PlayerMovementManager owner)
    {
        m_Owner = owner;
    }

    public override void MyOnCollisionEnter2D(Collision2D collision)
    {
    }

    public override void OnEnd()
    {
        if (!m_Somersaulting)
        {
            m_Owner.GraphicsPivot.transform.localPosition = new Vector3(0, 0.5f);
            m_Owner.GraphicsPivot.transform.eulerAngles = Vector3.zero;
        }
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
        m_Adjuster = m_Owner.GraphicsPivot.transform.localPosition;

        if(PassedThroughInteraction)
        {
            m_Owner.CurrentDirection = EDirection.Up;
            PassedThroughInteraction = false;
        }
    }

    public override void OnUpdate()
    {
        if (m_Owner.InputDirection.magnitude != 0)
        {
            if (m_Owner.InputDirection.x != 0)
            {
                if (m_Owner.ForwardCheckOfWall(m_Owner.InputDirection, 0.26f, m_Owner.transform.position + (Vector3.up / 2)))
                {
                    Move();
                }
            }
            else
            {
                if (m_Owner.ForwardCheckOfWall(m_Owner.InputDirection, 0.6f, m_Owner.transform.position + (Vector3.up / 2)))
                {
                    Move();
                }
            }

        }
        else if(m_IsMoving)
        {
            if (m_CurrentDirection.magnitude != 0)
            {
                if (m_Owner.Rigidbody.velocity == Vector2.zero)
                {
                    if (m_CurrentDirection.y != 0)
                    {
                        if (!m_Owner.ForwardCheckOfWall(m_CurrentDirection, 1f))
                        {
                            StopMovement();
                        }
                    }
                    else
                    {
                        if (!m_Owner.ForwardCheckOfWall(m_CurrentDirection, 0.5f, m_Owner.transform.position + (Vector3.up / 2))) // questo non detecta le piattaforme perchè sono troppo in alto, da rivedere
                        {
                            StopMovement();
                        }
                    }
                }
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
                FlipSprite(270, 90, 0, 180);
                m_Owner.Skeleton.loop = false;
                m_Owner.Skeleton.AnimationName = "GravitàApice";
            }
        }
    }

    public void StopMovement()
    {
        m_Somersaulting = true;
        m_CurrentDirection = Vector3.zero;
        m_IsMoving = false;
        m_Owner.StateMachine.SetState(EPlayerState.Somersault);
    }

    private void Movement()
    {
        m_Owner.Rigidbody.velocity = m_Owner.SpeedInZeroGravity * Time.fixedDeltaTime * m_CurrentDirection;
    }

    private void FlipSprite(int right, int left, int up, int down)
    {
        if(m_CurrentDirection.x > 0)
        {
            if(m_Owner.CurrentDirection == EDirection.Left)
                SetAdjuster(new Vector2(-0.56f, 0));
            else
                SetAdjuster(new Vector2(-0.28f, 0));

            m_Owner.RotatePlayer(right, m_Adjuster);

            m_Owner.CurrentDirection = EDirection.Right;
        }
        else if(m_CurrentDirection.y > 0)
        {
            m_Owner.CurrentDirection = EDirection.Up;
            SetAdjuster(Vector2.zero);
            m_Owner.RotatePlayer(up, m_Adjuster);

        }
        else if(m_CurrentDirection.x < 0)
        {
            if(m_Owner.CurrentDirection == EDirection.Right)
                SetAdjuster(new Vector2(0.56f, 0));
            else
                SetAdjuster(new Vector2(0.28f, 0));

            m_Owner.RotatePlayer(left, m_Adjuster);


            m_Owner.CurrentDirection = EDirection.Left;
        }
        else if(m_CurrentDirection.y < 0)
        {
            m_Owner.CurrentDirection = EDirection.Down;
            SetAdjuster(Vector2.zero);
            m_Owner.RotatePlayer(down, m_Adjuster);

        }
    }

    private void SetAdjuster(Vector2 newAdjuster)
    {
        if (m_Adjuster.x != 0 && m_Owner.CurrentDirection != EDirection.Left && m_Owner.CurrentDirection != EDirection.Right)
        {
            
            newAdjuster = new Vector2(-m_Adjuster.x, newAdjuster.y);
        }

        m_Adjuster = newAdjuster;
    }
}
