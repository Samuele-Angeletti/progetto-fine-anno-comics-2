using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class LandingPlayerState : State
{
    private PlayerMovementManager m_Owner;

    public LandingPlayerState(PlayerMovementManager owner)
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
        Land();
    }

    public override void OnStart()
    {
        Debug.Log("STATO: LANDING");
        m_Owner.PlayerAnimator?.SetBool("Land", true);
    }

    public override void OnUpdate()
    {
        if(m_Owner.IsGrounded)
        {
            m_Owner.Rigidbody.velocity = new Vector2(m_Owner.Rigidbody.velocity.x, 0);
            m_Owner.StateMachine.SetState(EPlayerState.Walking);
        }
    }

    private void Land()
    {
        m_Owner.Rigidbody.velocity += new Vector2(m_Owner.Rigidbody.velocity.x, Vector2.up.y * m_Owner.GravityScale) * Time.fixedDeltaTime;
    }


}
