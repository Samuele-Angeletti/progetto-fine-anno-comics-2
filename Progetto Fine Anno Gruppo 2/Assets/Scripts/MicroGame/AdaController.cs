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
            Invoke("InitialSettings", 0.1f);
        }

        private void InitialSettings()
        {
            GameManagerPM.Instance.UISpawnInitialLifes(m_MaxLifes);
	        m_AdaLogic.ContinousMovement = true;
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
                PubSub.PubSub.Publish(new GameOverMicroGameMessage(false));
            }

            m_AdaLogic.MoveDirection(Vector2.zero);
            GameManagerPM.Instance.LoseLife();

        }

        public void PlaceOnSpawnPoint()
        {
            transform.position = m_SpawnPoint.position;
            GetComponent<CircleCollider2D>().enabled = true;
            m_Graphics.SetActive(true);

        }

        public void SpawnParticleEffect()
        {
            ParticleSystem p = Instantiate(m_VFXOnDeath, transform.position, Quaternion.identity);
            Destroy(p.gameObject, 2f);
        }
    }
}

