using ArchimedesMiniGame;
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
        [Header("Interaction Settings")]
        [SerializeField] List<EInteractionType> m_CallInteraction;
        [SerializeField] List<GameObject> m_InterestedObjectsOnInteraction;
        [Header("Dialogue Settings")]
        [SerializeField] DialogueHolderSO m_DialogueHolderSOs;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerMovementManager>() != null)
            {
                Interacter interacter = other.GetComponent<Interacter>();
                if (m_Active)
                {
                    if (m_OneShot) m_Active = false;
                    Invoke(interacter);

                }
            }
        }

        /// <summary>
        /// Chiama tutti gli eventi: UnityEvent, Interaction, Dialogue
        /// </summary>
        /// <param name="interacter">Interacter per dialogo</param>
        public void Invoke(Interacter interacter)
        {
            InvokeEvent();
            InvokeInteraction(interacter);
            InvokeDialogue();
        }

        /// <summary>
        /// Chiama il Dialogue
        /// </summary>
        public void InvokeDialogue()
        {
            if (m_DialogueHolderSOs != null)
                PubSub.PubSub.Publish(new StartDialogueMessage(m_DialogueHolderSOs.Dialogo,false));
        }

        /// <summary>
        /// Chiama l'Interaction
        /// </summary>
        /// <param name="interacter"></param>
        public void InvokeInteraction(Interacter interacter)
        {
            int i = 0;
            foreach (EInteractionType eInteraction in m_CallInteraction)
            {
                GameObject g = m_InterestedObjectsOnInteraction[i];
                if (g == null)
                {
                    g = new GameObject();
                    Destroy(g, 2f);
                }
                GameManager.Instance.GetButtonInteractionSO(eInteraction).Interact(g, interacter);
                i++;
            }
        }

        /// <summary>
        /// Chiama lo UnityEvent
        /// </summary>
        public void InvokeEvent()
        {
            m_EventOnTrigger.Invoke();
        }
    }
}
