using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using MainGame;
using Commons;

public class InteractingPlayerState : State
{
    private PlayerMovementManager m_Owner;
    private float m_TimePassed;
    private float m_CastTimeScale;
    private Interactable m_CurrentInteractable;
    public InteractingPlayerState(PlayerMovementManager playerMovementManager)
    {
        m_Owner = playerMovementManager;
    }

    public override void MyOnCollisionEnter2D(Collision2D collision)
    {

    }

    public override void OnEnd()
    {
        m_Owner.Skeleton.timeScale = m_CastTimeScale;
        m_Owner.PassingFromZeroG = false;
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnStart()
    {
        Debug.Log("STATO: INTERACTING");
        m_TimePassed = 0;
        m_Owner.Skeleton.loop = false;
        m_CastTimeScale = m_Owner.Skeleton.timeScale;
        m_Owner.Skeleton.timeScale = 2f;
        m_Owner.Skeleton.AnimationName = "PremerePulsante";
        m_CurrentInteractable = m_Owner.Interacter.GetInteractable();
        if(m_CurrentInteractable != null)
        {
            if(m_CurrentInteractable.transform.eulerAngles.y > 0 && m_Owner.Flipped)
            {
                m_Owner.FlipSpriteOnX(false);
            }
            else if(m_CurrentInteractable.transform.eulerAngles.y <= 0 && !m_Owner.Flipped)
            {
                m_Owner.FlipSpriteOnX(true);
            }
        }
    }

    public override void OnUpdate()
    {
        m_TimePassed += Time.deltaTime;
        if(m_TimePassed >= m_Owner.InteractionAnimationSpeed)
        {
            if (m_Owner.Interacter.CurrentInteractable.InteractionType != EInteractionType.ZeroG)
            {
                if (m_Owner.PassingFromZeroG)
                    m_Owner.StateMachine.SetState(EPlayerState.ZeroG);
                else
                    m_Owner.StateMachine.SetState(EPlayerState.Walking);
            }
            m_CurrentInteractable?.Interact(m_Owner.Interacter);
        }
    }
}
