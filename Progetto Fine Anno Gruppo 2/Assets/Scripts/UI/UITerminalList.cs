using System;
using UnityEngine;
using TMPro;
using PubSub;
using System.Collections.Generic;

public class UITerminalList : Menu, ISubscriber
{
    [SerializeField] GameObject m_ListContainer;
    [Header("Prefab")]
    [SerializeField] GameObject m_ButtonTerminalPrefab;

    private List<string> m_ButtonsIDs = new List<string>();

    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(TerminalMessage));
    }

    public void AddNewButton(TerminalScriptableObject terminalScriptableObject)
    {
        GameObject newButton = Instantiate(m_ButtonTerminalPrefab, m_ListContainer.transform);
        newButton.GetComponent<ButtonActions>().SetTerminalSO(terminalScriptableObject);
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = terminalScriptableObject.Title;

        terminalScriptableObject.GenerateID();
        m_ButtonsIDs.Add(terminalScriptableObject.UniqueID);
    }

    public void OnDisableSubscribe()
    {

        PubSub.PubSub.Unsubscribe(this, typeof(TerminalMessage));
    }

    private void OnDestroy()
    {
        OnDisableSubscribe();
    }

    public void OnPublish(IMessage message)
    {
        if(message is TerminalMessage)
        {
            TerminalMessage terminalMessage = (TerminalMessage)message;

            if(!m_ButtonsIDs.Contains(terminalMessage.TerminalScriptableObject.UniqueID))
                AddNewButton(terminalMessage.TerminalScriptableObject);
            Close();
        }
    }
}
