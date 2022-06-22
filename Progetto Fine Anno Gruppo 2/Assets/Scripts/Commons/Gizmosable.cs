using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Commons
{
    public class Gizmosable : MonoBehaviour
    {
        [SerializeField] public GameObject InterestedObject;

#if UNITY_EDITOR
        [Header("Gizmos Settings")]
        [SerializeField] bool m_ActiveGizmos;
        [SerializeField] Color m_LineColor;
        [SerializeField] Color m_CircleColor;
        [SerializeField] float m_Radius;
        private void OnDrawGizmos()
        {
            if (m_ActiveGizmos && InterestedObject != null)
            {
                Gizmos.color = m_LineColor;
                Gizmos.DrawLine(transform.position, InterestedObject.transform.position);
                Gizmos.color = m_CircleColor;
                Gizmos.DrawSphere(InterestedObject.transform.position, m_Radius);
            }
        }
#endif
    }
}
