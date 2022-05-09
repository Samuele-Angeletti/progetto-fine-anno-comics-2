using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class RunningPlayerState : State
{
    private PlayerMovementManager m_Owner;

    public RunningPlayerState(PlayerMovementManager owner)
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
        Debug.Log("STATO: RUNNING");
    }

    public override void OnUpdate()
    {

    }

}
