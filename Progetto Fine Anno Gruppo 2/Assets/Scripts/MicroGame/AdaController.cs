using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MicroGame
{
    public class AdaController : MonoBehaviour
    {

        [SerializeField] CharacterLogicPM m_AdaLogic;
        [SerializeField] int m_MaxLifes;
        [SerializeField] Animator m_Animator;
        [SerializeField] Transform m_SpawnPoint;
        [SerializeField] ParticleSystem m_VFXOnDeath;
        [SerializeField] GameObject m_Graphics;
        
        private int m_CurrentLifes;

        private void Start()
        {
            m_CurrentLifes = m_MaxLifes;
            GameManagerPM.Instance.UIUpdateLife(m_CurrentLifes);
        }

        internal void LoseLife()
        {
            m_CurrentLifes--;

            if(m_Animator != null)
            {
                m_Animator.SetTrigger("PlayDeath");
            }

            if(m_CurrentLifes <= 0)
            {
                PubSub.PubSub.Publish(new GameOverMicroGameMessage());
            }

            m_AdaLogic.MoveDirection(Vector2.zero);
            GameManagerPM.Instance.UIUpdateLife(m_CurrentLifes);

        }

        public void PlaceOnSpawnPoint()
        {
            transform.position = m_SpawnPoint.position;
            GetComponent<BoxCollider2D>().enabled = true;
            m_Graphics.SetActive(true);

        }

        public void SpawnParticleEffect()
        {
            ParticleSystem p = Instantiate(m_VFXOnDeath, transform.position, Quaternion.identity);
            Destroy(p.gameObject, 2f);
        }
    }
}

