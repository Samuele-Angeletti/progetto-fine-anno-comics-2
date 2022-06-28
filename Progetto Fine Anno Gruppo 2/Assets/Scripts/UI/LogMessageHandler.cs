using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogMessageHandler : MonoBehaviour
{
    [SerializeField] GameObject m_Container;
    [SerializeField] TextMeshProUGUI m_TextPrefab;

    public void SpawnMessage(string text, float timeDisplay)
    {
        TextMeshProUGUI message = Instantiate(m_TextPrefab, m_Container.transform);
        message.text = text;
        Destroy(message.gameObject, timeDisplay);
    }
}
