using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using System;

public class WalkingPlayerState : State
{
    private PlayerMovementManager m_Owner;

    public WalkingPlayerState(PlayerMovementManager owner)
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
        m_Owner.Rigidbody.velocity = m_Owner.Direction * m_Owner.MovementVelocity * Time.fixedDeltaTime;

    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
    }
}
