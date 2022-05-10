using MainGame;
using UnityEngine;

public class WalkingPlayerState : State
{
    private PlayerMovementManager m_Owner;
    private float m_TimePassed = 0;
    private bool m_OnDeceleration;
    public WalkingPlayerState(PlayerMovementManager owner)
    {
        m_Owner = owner;
    }

    public override void MyOnCollisionEnter2D(Collision2D collision)
    {

    }

    public override void OnEnd()
    {
        m_TimePassed = 0;
    }

    public override void OnFixedUpdate()
    {
        if (Mathf.Abs(m_Owner.Direction.magnitude) > 0)
            Movement();
        if (m_OnDeceleration)
        {
            Decelerate();
        }
    }

    private void Decelerate()
    {
        if (m_Owner.Rigidbody.velocity.x > 0)
        {
            m_Owner.Rigidbody.velocity -= new Vector2(m_Owner.InertiaDecelerator * Time.fixedDeltaTime, 0f);
            if (m_Owner.Rigidbody.velocity.x < 0)
            {
                m_Owner.Rigidbody.velocity = Vector3.zero;
                m_OnDeceleration = false;
            }
        }
        else if (m_Owner.Rigidbody.velocity.x < 0)
        {
            m_Owner.Rigidbody.velocity += new Vector2(m_Owner.InertiaDecelerator * Time.fixedDeltaTime, 0f);
            if (m_Owner.Rigidbody.velocity.x > 0)
            {
                m_Owner.Rigidbody.velocity = Vector3.zero;
                m_OnDeceleration = false;
            }
        }
    }

    public override void OnStart()
    {
        Debug.Log("STATO: WALKING");
        m_TimePassed = 0;
    }

    public override void OnUpdate()
    {
        if (m_Owner.IsJumping)
        {
            m_Owner.StateMachine.SetState(EPlayerState.Jumping);
        }
        if (!m_Owner.IsGrounded)
        {
            m_Owner.StateMachine.SetState(EPlayerState.Landing);
        }
        if (m_Owner.Direction.magnitude == 0)
        {
            m_TimePassed += Time.deltaTime;
            if (m_TimePassed >= m_Owner.InertiaTime)
            {
                m_OnDeceleration = true;
                m_TimePassed = 0;
            }
        }

        if(Mathf.Abs(m_Owner.Rigidbody.velocity.x) > 0)
            m_Owner.PlayerAnimator?.SetFloat("Walking", 1);
        else
            m_Owner.PlayerAnimator?.SetFloat("Walking", -1);
    }

    private void Movement()
    {
        m_Owner.Direction = m_Owner.Direction.x > 0 ? new Vector2(1, 0) : new Vector2(-1, 0);

        if (Mathf.Abs(m_Owner.Rigidbody.velocity.x) > m_Owner.MaxSpeed)
        {
            m_Owner.Rigidbody.velocity = new Vector2(Mathf.Sign(m_Owner.Rigidbody.velocity.x) * m_Owner.MaxSpeed, m_Owner.Rigidbody.velocity.y);
        }

        if (m_Owner.Direction.x != 0)
        {

            if (m_Owner.Rigidbody.velocity.x > 0)
            {
                m_Owner.FlipSpriteOnX(true);
            }
            if (m_Owner.Rigidbody.velocity.x < -0.1)
            {
                m_Owner.FlipSpriteOnX(false);
            }
        }
        m_Owner.Rigidbody.velocity = m_Owner.Direction * m_Owner.MaxSpeed * Time.fixedDeltaTime;
    }

}
