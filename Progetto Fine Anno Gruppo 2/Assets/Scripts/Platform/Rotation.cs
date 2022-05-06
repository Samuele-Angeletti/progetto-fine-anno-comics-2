using UnityEngine;

namespace MainGame
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] float m_RotationSpeed;
        [Tooltip("Le direzioni UP e DOWN seguono rispettivamente Right e Left, ovvero in senso orario")]
        [SerializeField] EDirection m_Direction;

        private bool m_OnRotation;
        private Vector3 m_Destination;

        private void Update()
        {
            if (m_OnRotation)
            {
                if (m_Direction == EDirection.Up || m_Direction == EDirection.Right)
                {
                    Rotate(Vector3.back);

                    if (transform.eulerAngles.z <= m_Destination.z)
                    {
                        m_OnRotation = false;

                        if(m_Destination.z == 1)
                        {
                            m_Destination = Vector3.zero;
                        }

                        transform.eulerAngles = m_Destination;
                    }
                }
                else
                {
                    Rotate(Vector3.forward);

                    if (transform.eulerAngles.z >= m_Destination.z)
                    {
                        m_OnRotation = false;

                        if (m_Destination.z == 359)
                        {
                            m_Destination = Vector3.zero;
                        }

                        transform.eulerAngles = m_Destination;
                    }
                }
            }
        }

        private void Rotate(Vector3 direction)
        {
            transform.Rotate(direction * m_RotationSpeed * Time.deltaTime);
        }

        public void SetRotationDestination()
        {
            if (!m_OnRotation)
            {
                m_OnRotation = true;

                if (m_Direction == EDirection.Up || m_Direction == EDirection.Right)
                {
                    m_Destination = transform.eulerAngles - new Vector3(0, 0, 90);
                    if (m_Destination.z < 0)
                    {
                        m_Destination = new Vector3(0, 0, 270);
                    }
                    if (m_Destination.z == 0)
                    {
                        m_Destination = new Vector3(0, 0, 1);
                    }
                }
                else
                {
                    m_Destination = transform.eulerAngles + new Vector3(0, 0, 90);
                    if (m_Destination.z > 360)
                    {
                        m_Destination = new Vector3(0, 0, 90);
                    }
                    if (m_Destination.z == 360)
                    {
                        m_Destination = new Vector3(0, 0, 359);
                    }
                }
            }
        }
    }
}
