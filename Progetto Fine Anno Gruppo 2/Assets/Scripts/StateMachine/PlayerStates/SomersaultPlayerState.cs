using MainGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomersaultPlayerState : State
{
    private PlayerMovementManager m_Owner;
    private float m_TimePassed;
    private float m_StartTimeScale;
    public SomersaultPlayerState(PlayerMovementManager owner)
    {
        m_Owner = owner;
    }
    public override void MyOnCollisionEnter2D(Collision2D collision)
    {
    }

    public override void OnEnd()
    {
        m_Owner.Skeleton.timeScale = m_StartTimeScale;
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnStart()
    {
        Debug.Log("STATO: SOMERSAULT");
        m_TimePassed = 0;
        m_Owner.Skeleton.loop = false;
        m_StartTimeScale = m_Owner.Skeleton.timeScale;
        m_Owner.Skeleton.timeScale = 3;
        //m_Owner.Skeleton.AnimationName = "CapriolaGravitàDue";
        m_Owner.Skeleton.AnimationName = "Idol";
    }

    public override void OnUpdate()
    {
        if (m_Owner.InputDirection.magnitude > 0)
        {
            m_Owner.StateMachine.SetState(EPlayerState.ZeroG);
            return;
        }
        
        m_TimePassed += Time.deltaTime;
        if (m_TimePassed >= 1f)
        {
            m_Owner.StateMachine.SetState(EPlayerState.ZeroG);
        }
    }
}
