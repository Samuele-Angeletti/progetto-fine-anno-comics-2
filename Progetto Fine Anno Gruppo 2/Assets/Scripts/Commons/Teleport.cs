using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public class Teleport : MonoBehaviour
    {
        [Header("Porta d'uscita necessaria")]
        [SerializeField] Teleport m_ExitDoor;

        [HideInInspector]
        public bool CurrentlyExitDoor;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ITeleportable teleportable = collision.gameObject.GetComponent<ITeleportable>();
            if (teleportable != null)
            {
                if (CurrentlyExitDoor) return;

                m_ExitDoor.CurrentlyExitDoor = true;
                collision.gameObject.transform.position = m_ExitDoor.transform.position;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            ITeleportable teleportable = collision.gameObject.GetComponent<ITeleportable>();
            if (teleportable != null)
            {
                CurrentlyExitDoor = false;
            }
        }
    }
}

