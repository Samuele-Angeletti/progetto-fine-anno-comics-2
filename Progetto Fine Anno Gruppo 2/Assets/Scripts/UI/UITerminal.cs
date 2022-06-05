using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
using TMPro;
namespace MainGame
{
    public class UITerminal : MonoBehaviour, ISubscriber
    {
        [Header("Scene References")]
        [SerializeField] GameObject m_Background;
        [SerializeField] TextMeshProUGUI m_TitleText;
        [SerializeField] TextMeshProUGUI m_ProgressiveNoText;
        [SerializeField] TextMeshProUGUI m_AuthorText;
        [SerializeField] TextMeshProUGUI m_ContentText;

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(TerminalMessage));
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(TerminalMessage));
        }

        public void OnPublish(IMessage message)
        {
            if(message is TerminalMessage)
            {
                TerminalMessage terminalMessage = (TerminalMessage)message;
                m_TitleText.text = terminalMessage.TerminalScriptableObject.Title;
                m_AuthorText.text = terminalMessage.TerminalScriptableObject.Author;
                m_ProgressiveNoText.text = terminalMessage.TerminalScriptableObject.ProgressiveNumber;
                m_ContentText.text = terminalMessage.TerminalScriptableObject.Content;

                m_Background.SetActive(true);

            }
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }
    }
}
