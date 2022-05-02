using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchimedesMiniGame
{
    public class Module : MonoBehaviour
    {
        [Header("Main settings")]
        [SerializeField] float m_Acceleration;
        [SerializeField] float m_MaxSpeed;
        [SerializeField] float m_RotationSpeed;

        private Rigidbody2D m_Rigidbody;
        private Vector2 m_Direction;
        private Vector2 m_RotationDirection;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            m_Rigidbody.velocity += -m_Direction.normalized * m_Acceleration * Time.fixedDeltaTime;

            if(m_Rigidbody.velocity.x >= m_MaxSpeed)
            {
                m_Rigidbody.velocity = new Vector2(m_MaxSpeed, m_Rigidbody.velocity.y);
            }

            if (m_Rigidbody.velocity.y >= m_MaxSpeed)
            {
                m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_MaxSpeed);
            }
        }

        private void Update()
        {
            transform.eulerAngles += new Vector3(0, 0, m_RotationDirection.x) * Time.deltaTime * m_RotationSpeed;
        }

        internal void Rotate(Vector2 rotationVector)
        {
            m_RotationDirection = rotationVector.normalized;
        }

        internal void AddForce(Vector2 movementVector)
        {
            m_Direction = transform.forward - new Vector3(movementVector.x, movementVector.y);
        }
    }
}
