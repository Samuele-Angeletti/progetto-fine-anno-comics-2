using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchimedesMiniGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DockingPoint : MonoBehaviour
    {
        public bool IsActive;
        [SerializeField] Transform m_DockingPivot;
        [Tooltip("Questo fa riferimento alla rotazione del punto d'accesso nell'asse Z. Up = 0, Right = 90, Down = 180, Left = 270")]
        [SerializeField] EDirection m_PivotOrientation;

        public Transform DockingPivot => m_DockingPivot;
        public EDirection Orientation => m_PivotOrientation;
    }
}
