using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchimedesMiniGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DockingPoint : MonoBehaviour
    {
        public bool IsActive;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Module m = collision.GetComponent<Module>();
            if(m != null)
            {
                GameManagerES.Instance.ActiveDockingAttemptButton(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Module m = collision.GetComponent<Module>();
            if (m != null)
            {
                GameManagerES.Instance.ActiveDockingAttemptButton(false);
            }
        }
    }
}
