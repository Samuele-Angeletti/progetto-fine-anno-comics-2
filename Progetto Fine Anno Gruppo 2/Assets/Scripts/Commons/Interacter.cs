using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Commons
{
    public class Interacter : MonoBehaviour
    {
        [SerializeField] GameObject m_UIText;
        [SerializeField] bool m_AnimatedUI;
        [SerializeField] float m_FadeTime;

        private TextMeshProUGUI m_Text;
        private RawImage m_Background;

        private IInteractable m_CurrentInteractable;
        private bool m_ActiveUI;
        private float m_TimePassed;

        private void Awake()
        {
            m_Text = m_UIText.GetComponentInChildren<TextMeshProUGUI>();
            m_Background = m_UIText.GetComponentInChildren<RawImage>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.ShowUI(true);
                m_ActiveUI = true;
                m_CurrentInteractable = interactable;
                m_UIText.SetActive(true);
                m_TimePassed = 0;
                AddColorToUI(0);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.ShowUI(false);
                m_CurrentInteractable = null;
                m_UIText.SetActive(false);
                m_ActiveUI = false;
                AddColorToUI(0);
                m_TimePassed = 0;
            }

        }

        public IInteractable GetInteractable()
        {
            return m_CurrentInteractable;
        }

        private void Update()
        {
            if(m_ActiveUI)
            {
                if(m_AnimatedUI)
                {
                    m_TimePassed += Time.deltaTime;
                    AddColorToUI(m_TimePassed / m_FadeTime);
                    if(m_TimePassed >= m_FadeTime)
                    {
                        m_TimePassed = 0;
                        m_ActiveUI = false;
                        AddColorToUI(1);
                    }
                }
                else
                {
                    AddColorToUI(1);
                    m_ActiveUI = false;
                }
            }
        }

        private void AddColorToUI(float amount)
        {
            Color colorText = m_Text.color;
            Color colorImage = m_Background.color;
            colorText.a = amount;
            colorImage.a = amount / 2;
            m_Text.color = colorText;
            m_Background.color = colorImage;
        }
    }
}