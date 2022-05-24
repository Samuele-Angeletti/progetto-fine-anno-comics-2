using MainGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandedPlayerState : State
{
    private PlayerMovementManager m_Owner;
    private float m_TimePassed;

    public LandedPlayerState(PlayerMovementManager owner)
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
    }

    public override void OnStart()
    {
        Debug.Log("STATO: LANDED");
        m_TimePassed = 0;
        m_Owner.Skeleton.loop = false;
        m_Owner.Skeleton.AnimationName = "SaltoTermineDue";
    }

    public override void OnUpdate()
    {
        if(m_Owner.InputDirection.magnitude > 0)
        {
            m_Owner.StateMachine.SetState(EPlayerState.Walking);
            return;
        }
        if (m_Owner.IsJumping)
        {
            m_Owner.StateMachine.SetState(EPlayerState.Jumping);
            return;
        }
        m_TimePassed += Time.deltaTime;
        if(m_TimePassed >= 1f)
        {
            m_Owner.StateMachine.SetState(EPlayerState.Walking);
        }
    }
}
