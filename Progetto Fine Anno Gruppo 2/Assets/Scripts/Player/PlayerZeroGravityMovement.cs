using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZeroGravityMovement : State
{
    private GenericStateMachine<EPlayerState> m_stateMachine;
    private InputControls inputActions;
    private Rigidbody2D m_rigidbody;
    private Collider2D m_playerCollider;
    private Animator m_playerAnimator;
    private SpriteRenderer m_spriteRenderer;
    private bool isMoving;
    private float m_speed;

    public PlayerZeroGravityMovement(GenericStateMachine<EPlayerState> stateMachine,Rigidbody2D playerRb2D,Collider2D playerCollider,Animator playerAnimator,SpriteRenderer playerRenderer,float zeroGravitySpeed)
    {
        inputActions = new InputControls();
        m_stateMachine = stateMachine;
        m_rigidbody = playerRb2D;
        m_playerCollider = playerCollider;
        m_playerAnimator = playerAnimator;
        m_spriteRenderer = playerRenderer;
        m_speed = zeroGravitySpeed;

    }




    private void Movement()
    {
        if (isMoving) return;
        if (!isMoving)
        {
            Vector2 direction = inputActions.Player.ZeroGravityMovement.ReadValue<Vector2>();
            m_rigidbody.velocity = direction * m_speed * Time.deltaTime;
            isMoving = true;
        }
    }

    public override void OnStart()
    {
        inputActions?.Player.ZeroGravityMovement.Enable();
        m_rigidbody.gravityScale = 0;
        Debug.Log("Sono nello stato senza gravità");
    }

    public override void OnUpdate()
    {
    }

    public override void OnEnd()
    {
        inputActions?.Player.ZeroGravityMovement.Disable();

    }

    public override void MyOnCollisionEnter2D(Collision2D collision)
    {
        m_rigidbody.velocity = Vector3.zero;
        isMoving = false;
    }

    public override void OnFixedUpdate()
    {
        isMoving = m_rigidbody.velocity == Vector2.zero ? false : true;
        Movement();
    }
}
