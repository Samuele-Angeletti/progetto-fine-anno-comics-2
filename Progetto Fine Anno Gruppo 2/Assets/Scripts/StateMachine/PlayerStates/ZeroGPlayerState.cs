using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class ZeroGPlayerState : State
{
    private PlayerMovementManager m_Owner;

    public ZeroGPlayerState(PlayerMovementManager owner)
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
        Debug.Log("STATO: ZERO G");
    }

    public override void OnUpdate()
    {

    }

}
