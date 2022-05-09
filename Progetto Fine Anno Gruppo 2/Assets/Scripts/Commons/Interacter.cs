using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public class Interacter : MonoBehaviour
    {
        IInteractable m_CurrentInteractable;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.ShowUI(true);

                m_CurrentInteractable = interactable;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.ShowUI(false);
                m_CurrentInteractable = null;
            }

        }

        public IInteractable GetInteractable()
        {
            return m_CurrentInteractable;
        }
    }
}
