using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

namespace MicroGame
{
    public class Enemy : MonoBehaviour, ISubscriber
    {
        [SerializeField] SpriteRenderer m_SpriteRenderer;
        [SerializeField] Color m_ColorOnCatchable;
        [SerializeField] float m_TimeOnCatchable;

        private Color m_BaseColor;
        private bool m_Catchable;
        private float m_TimePassed;

        private void Awake()
        {
            m_BaseColor = m_SpriteRenderer.color;
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(PowerUpMessage));
        }

        public void OnPublish(IMessage message)
        {
            if(message is PowerUpMessage)
            {
                m_Catchable = true;
                m_SpriteRenderer.color = m_ColorOnCatchable;
            }
        }

        private void Update()
        {
            if(m_Catchable)
            {
                m_TimePassed += Time.deltaTime;
                if(m_TimePassed >= m_TimeOnCatchable)
                {
                    m_TimePassed = 0;
                    m_SpriteRenderer.color = m_BaseColor;
                    m_Catchable = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            AdaController adaController = collision.collider.gameObject.GetComponent<AdaController>();
            if(adaController != null)
            {
                if(m_Catchable)
                {
                    Destroy(gameObject);
                }
                else
                {
                    adaController.LoseLife();
                }
            }
        }

        private void OnDestroy()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(PowerUpMessage));
        }
    }
}


