using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MainGame
{
    public class EventArea : MonoBehaviour
    {
        [SerializeField] bool m_Active;
        [SerializeField] bool m_OneShot;
        [SerializeField] UnityEvent m_EventOnTrigger;
        [SerializeField] List<EInteractionType> m_CallInteraction;
        [SerializeField] List<GameObject> m_InterestedObjectsOnInteraction;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerMovementManager>() != null)
            {
                Interacter interacter = other.GetComponent<Interacter>();
                if (m_Active)
                {
                    if (m_OneShot) m_Active = false;

                    m_EventOnTrigger.Invoke();
                    int i = 0;
                    foreach(EInteractionType eInteraction in m_CallInteraction)
                    {
                        GameObject g = m_InterestedObjectsOnInteraction[i];
                        if(g == null)
                        {
                            g = new GameObject();
                            Destroy(g, 2f);
                        }
                        GameManager.Instance.GetButtonInteractionSO(eInteraction).Interact(g, interacter);
                        i++;
                    }
                }
            }
        }
    }
}
