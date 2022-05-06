using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MicroGame
{
    public class CharacterLogic : MonoBehaviour, ITeleportable
    {
        [SerializeField] Rigidbody2D m_Rigidbody;
        [SerializeField] float m_Speed;
        [SerializeField] EController controller;
        [SerializeField] Vector2 m_StoppingDistance;

        [Header("Solo per IA")]
        [Tooltip("Tempo massimo per la ricerca di una nuova direzione. Il tempo è all'interno di una Coroutine ed è randomico tra 0 e questo numero")]
        [SerializeField] float m_MaxRandomTime;

        private Vector2 m_Direction;       
        private Vector2 m_NextDirection;       


        private void Start()
        {
            if(controller == EController.IA)
            {
                StartCoroutine(SearchNewDirection());
            }
        }

        private void FixedUpdate()
        {
            m_Rigidbody.velocity = m_Direction * m_Speed * Time.fixedDeltaTime;
        }

        public void NewDirection(Vector2 newDirection)
        {
            if (ForwardCheck(newDirection.normalized))
            {
                m_Direction = newDirection.normalized;
            }
            else if(m_Direction != Vector2.zero) // if asked for a new direction but the character is already moving and can't go in that direction, so this stores the next direction to go asap
            {
                m_NextDirection = newDirection.normalized;
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
            else
            {
                if(m_NextDirection != Vector2.zero) // this control is for the next move that the player asked for.
                {
                    if(ForwardCheck(m_NextDirection))
                    {
                        m_Direction = m_NextDirection;
                        m_NextDirection = Vector2.zero;
                    }
                }
            }

            if (!ForwardCheck(m_Direction))
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

        public bool ForwardCheck(Vector2 direction)
        {
            RaycastHit2D[] raycastHits = new RaycastHit2D[10];
            int hits = Physics2D.BoxCastNonAlloc(transform.position, m_StoppingDistance, GetAngle(direction), direction, raycastHits, 0.6f);
            
            if(hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    if(raycastHits[i].collider.GetComponent<Wall>() != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private float GetAngle(Vector2 direction)
        {
            if(direction.x > 0)
            {
                return 90;
            }
            else if(direction.y > 0)
            {
                return 180;
            }
            else if(direction.x < 0)
            {
                return -90;
            }
            else
            {
                return -180;
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
