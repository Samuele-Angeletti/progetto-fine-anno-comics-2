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

        [Header("Solo per IA")]
        [Tooltip("Tempo massimo per la ricerca di una nuova direzione. Il tempo è all'interno di una Coroutine ed è randomico tra 0 e questo numero")]
        [SerializeField] float m_MaxRandomTime;

        private Vector2 m_Direction;
        private Wall m_CurrentWall;
        
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
            m_Direction = newDirection.normalized;
        }

        private void Update()
        {
            if(controller == EController.IA)
            {
                if(m_Direction == Vector2.zero)
                {
                    FindNextDirection();
                }
            }
        }

        private void FindNextDirection()
        {
            bool find = false;
            while(!find)
            {
                find = true;
                m_Direction = RandomDirection();
                RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, m_Direction, 1f);
                for (int i = 0; i < raycastHits.Length; i++)
                {
                    if(raycastHits[i].collider.gameObject.GetComponent<Wall>() != null)
                    {
                        find = false;
                    }
                }
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_CurrentWall == collision.gameObject.GetComponent<Wall>())
                return;
            else
                m_CurrentWall = collision.gameObject.GetComponent<Wall>();

            if (m_CurrentWall != null) m_Direction = Vector3.zero;
        }

        private IEnumerator SearchNewDirection()
        {
            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(0, m_MaxRandomTime));
                FindNextDirection();
            }
        }
    }
}
