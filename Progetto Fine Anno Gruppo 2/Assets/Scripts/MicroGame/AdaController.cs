using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MicroGame
{
    public class AdaController : MonoBehaviour
    {
        #region INPUTS
        public InputControls inputControls;

        private void Awake()
        {
            inputControls = new InputControls();

            inputControls.Player.Enable();

            inputControls.Player.Movement.started += StartMovement;
        }


        private void StartMovement(InputAction.CallbackContext obj)
        {
            m_AdaLogic.NewDirection(obj.ReadValue<Vector2>());
        }
        #endregion

        [SerializeField] CharacterLogic m_AdaLogic;
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

            m_AdaLogic.NewDirection(Vector2.zero);
            GameManagerPM.Instance.UIUpdateLife(m_CurrentLifes);
            inputControls.Player.Disable();
        }

        public void PlaceOnSpawnPoint()
        {
            transform.position = m_SpawnPoint.position;
            GetComponent<BoxCollider2D>().enabled = true;
            m_Graphics.SetActive(true);

            inputControls.Player.Enable();
        }

        public void SpawnParticleEffect()
        {
            ParticleSystem p = Instantiate(m_VFXOnDeath, transform.position, Quaternion.identity);
            Destroy(p.gameObject, 2f);
        }
    }
}

