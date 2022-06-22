using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
namespace MicroGame
{
    public class CharacterLogicPM : Controllable, ITeleportable
    {
        [Space(10)]
        [SerializeField] Rigidbody2D m_Rigidbody;
        [SerializeField] float m_Speed;
        [SerializeField] EController controller;

        [Header("Solo per IA")]
        [Tooltip("Tempo massimo per la ricerca di una nuova direzione. Il tempo � all'interno di una Coroutine ed � randomico tra 0 e questo numero")]
        [SerializeField] float m_MaxRandomTime;

        private Vector3 m_Destination;
        private bool m_OnGoing;
        private void Start()
        {
            if(controller == EController.IA)
            {
                StartCoroutine(SearchNewDirection());
            }
        }

        private void FixedUpdate()
        {
            if (controller == EController.Human)
            {
                Vector3 direction = m_Destination - transform.position;
                m_Rigidbody.velocity = direction.normalized * m_Speed * Time.fixedDeltaTime;
                if (Vector3.Distance(transform.position, m_Destination) < 0.1f)
                {
                    
                    transform.position = m_Destination;
                    m_Rigidbody.velocity = Vector3.zero;
                    m_OnGoing = false;
                }
            }
            else
            {
                m_Rigidbody.velocity = m_Direction * m_Speed * Time.fixedDeltaTime;
            }
        }

        public override void MoveDirection(Vector2 newDirection)
        {
            if (ForwardCheckOfWall(newDirection.normalized))
            {
                m_Direction = newDirection.normalized;
                
                if(!m_OnGoing || newDirection.magnitude > 0)
                    m_Destination = new Vector3(m_Direction.x, m_Direction.y) + transform.position;

                if (newDirection.magnitude > 0) m_OnGoing = true;
            }
        }

        private void Update()
        {
            if (controller == EController.IA)
            {
                if (m_Direction == Vector2.zero)
                {
                    m_Direction = RandomDirection();
                }
            }

            if (!ForwardCheckOfWall(m_Direction))
            {
                m_Direction = Vector2.zero;
            }
            
        }

        private Vector2 RandomDirection()
        {
            switch(UnityEngine.Random.Range(0,4))
            {
                case 0:
                    return Vector2.up;
                case 1: 
                    return Vector2.down;
                case 2:
                    return Vector2.left;
                case 3:
                    return Vector2.right;
                default:
                    return Vector2.zero;
            }
        }


        private IEnumerator SearchNewDirection()
        {
            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(0, m_MaxRandomTime));
                m_Direction = RandomDirection();
            }
        }

    }
}
