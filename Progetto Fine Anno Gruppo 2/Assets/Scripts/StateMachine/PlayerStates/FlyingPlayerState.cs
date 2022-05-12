using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class FlyingPlayerState : State
{
    private PlayerMovementManager m_Owner;

    public FlyingPlayerState(PlayerMovementManager owner)
    {
        m_Owner = owner;
    }

    public override void MyOnCollisionEnter2D(Collision2D collision)
    {

    }

    public override void OnEnd()
    {
        m_Owner.Rigidbody.gravityScale = 0;
    }

    public override void OnFixedUpdate()
    {
        m_Owner.InputDirection = new Vector2(m_Owner.InputDirection.x, 0);
        m_Owner.Movement();
    }

    public override void OnStart()
    {
        Debug.Log("STATE: FLYING");
        m_Owner.Rigidbody.gravityScale = 1;
    }

    public override void OnUpdate()
    {

    }
}
