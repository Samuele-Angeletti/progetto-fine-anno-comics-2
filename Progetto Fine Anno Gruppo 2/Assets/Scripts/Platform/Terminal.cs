using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
namespace MainGame
{
    public class Terminal : MonoBehaviour
    {
        [SerializeField] TerminalScriptableObject m_TerminalContent;

        public void ReadTerminal()
        {
            PubSub.PubSub.Publish(new TerminalMessage(m_TerminalContent));
            PubSub.PubSub.Publish(new PauseGameMessage());
        }
    }
}
