using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using TMPro;

public class UIMainDisplay : MonoBehaviour
{
    [SerializeField] GameObject m_DialogueBackground;
    [SerializeField] GameObject m_TerminalBackground;

    public void ActiveDialogueBackground(bool active)
    {
        m_DialogueBackground.SetActive(active);
    }

    public void ActiveTerminalBackground(bool active)
    {
        m_TerminalBackground.SetActive(active);
    }
}