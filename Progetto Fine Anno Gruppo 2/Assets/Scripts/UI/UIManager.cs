using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using System;

public class UIManager : MonoBehaviour, ISubscriber
{
	#region SINGLETON PATTERN
	public static UIManager m_instance;
	public static UIManager Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<UIManager>();

				if (m_instance == null)
				{
					GameObject container = new GameObject("GameManager");
					m_instance = container.AddComponent<UIManager>();
				}
			}

			return m_instance;
		}
	}
	#endregion

	[Header("Scene References")]
	[SerializeField] UIMainMenu m_MainMenu;
	[SerializeField] UISettingsMenu m_SettingsMenu;
	[SerializeField] UIPauseMenu m_PauseMenu;
	[SerializeField] UIGameOverMenu m_GameOverMenu;
	[SerializeField] UIConfirmChoiceMenu m_ConfirmChoiseMenu;
	[SerializeField] UIMainDisplay m_MainDisplay;
	[SerializeField] UIExternal m_ExternalUI;
	[SerializeField] UITerminalList m_TerminalListMenu;
	[SerializeField] UIPacManInterface m_PacManInterface;

	private Menu m_CurrentMenu;
	private bool m_Paused;
	private bool m_TerminalActive;
	public UIPacManInterface PacManInterface => m_PacManInterface;
    private void Awake()
    {
		if (m_instance == null)
		{
			m_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}

    void Start()
	{
		PubSub.PubSub.Subscribe(this, typeof(OpenMenuMessage));
		PubSub.PubSub.Subscribe(this, typeof(CloseAllMenusMessage));
		PubSub.PubSub.Subscribe(this, typeof(TerminalMessage));
		PubSub.PubSub.Subscribe(this, typeof(ResumeGameMessage));
	}

	public void OnPublish(IMessage message)
	{
		if (message is OpenMenuMessage)
		{
			if(m_TerminalActive)
            {
                CloseTerminalView();
            }

            OpenMenuMessage openMenu = (OpenMenuMessage)message;
			OpenMenu(openMenu.MenuType);
		}
		else if(message is CloseAllMenusMessage)
        {
			m_SettingsMenu.Close();
			m_PauseMenu.Close();
			m_ConfirmChoiseMenu.Close();
			m_TerminalListMenu.Close();
			m_CurrentMenu = null;
			m_Paused = false;
			CloseTerminalView();
		}
		else if(message is TerminalMessage)
        {
			m_TerminalActive = true;
        }
		else if(message is ResumeGameMessage)
        {
			if(m_TerminalActive)
            {
				m_Paused = false;
				m_TerminalActive = false;
            }
        }
	}

    internal void SetQuest(string messageOnScreen)
    {
		m_PauseMenu.SetQuestText($"OBIETTIVO: {messageOnScreen}");
    }

    private void CloseTerminalView()
    {
        m_TerminalActive = false;
        m_MainDisplay.ActiveTerminalBackground(false);
    }

    private void OpenMenu(EMenu menuToOpen)
	{
		if(m_CurrentMenu == GetMenuByEnum(menuToOpen) && menuToOpen != EMenu.Pause)
        {
			m_CurrentMenu.Close();
			m_CurrentMenu = null;
			return;
		}

		if (!m_Paused)
		{
			if (m_CurrentMenu == m_PauseMenu)
			{
				if (menuToOpen != EMenu.Pause)
				{
					m_CurrentMenu.Hide();
					m_Paused = true;
				}
				else
                {
					m_CurrentMenu.Close();
					m_CurrentMenu = null;
					return;
				}
			}
		}
		else if(menuToOpen != EMenu.Main)
        {
			m_CurrentMenu.Close();
			m_Paused = false;
			m_PauseMenu.Show();
			m_CurrentMenu = m_PauseMenu;
			return;
        }

		m_CurrentMenu = GetMenuByEnum(menuToOpen);
		m_CurrentMenu.Open();
	}

	private Menu GetMenuByEnum(EMenu menu)
	{
		switch (menu)
		{
			case EMenu.Main:
				return m_MainMenu;
			case EMenu.Settings:
				return m_SettingsMenu;
			case EMenu.Pause:
				return m_PauseMenu;
			case EMenu.GameOver:
				return m_GameOverMenu;
			case EMenu.ConfirmChoise:
				return m_ConfirmChoiseMenu;
			case EMenu.TerminalList:
				return m_TerminalListMenu;
			default:
				return null;
		}
	}

	public void OnDisableSubscribe()
	{
		PubSub.PubSub.Unsubscribe(this, typeof(OpenMenuMessage));
		PubSub.PubSub.Unsubscribe(this, typeof(CloseAllMenusMessage));
		PubSub.PubSub.Unsubscribe(this, typeof(TerminalMessage));
		PubSub.PubSub.Unsubscribe(this, typeof(ResumeGameMessage));
	}

	private void OnDestroy()
	{
		OnDisableSubscribe();
	}

	public void OpenPacManInterface(bool active)
    {
		m_PacManInterface.gameObject.SetActive(active);
    }
}