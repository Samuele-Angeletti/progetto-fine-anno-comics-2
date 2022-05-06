using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class UIManager : MonoBehaviour, ISubscriber
{


	[Header("Scene References")]
	[SerializeField] UIMainMenu m_MainMenu;
	[SerializeField] UISettingsMenu m_SettingsMenu;
	[SerializeField] UIPauseMenu m_PauseMenu;
	[SerializeField] UIGameOverMenu m_GameOverMenu;
	[SerializeField] UIConfirmChoiceMenu m_ConfirmChoiseMenu;
	[SerializeField] UIMainDisplay m_MainDisplay;

	private Menu m_CurrentMenu;
	private bool m_Paused;
	void Start()
	{
		PubSub.PubSub.Subscribe(this, typeof(OpenMenuMessage));
		PubSub.PubSub.Subscribe(this, typeof(CloseAllMenusMessage));
	}

	public void OnPublish(IMessage message)
	{
		if (message is OpenMenuMessage)
		{
			OpenMenuMessage openMenu = (OpenMenuMessage)message;
			OpenMenu(openMenu.MenuType);
		}
		else if(message is CloseAllMenusMessage)
        {
			m_SettingsMenu.Close();
			m_PauseMenu.Close();
			m_ConfirmChoiseMenu.Close();
			m_CurrentMenu = null;
			m_Paused = false;

		}
	}

	private void OpenMenu(EMenu menuToOpen)
	{
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
			default:
				return null;
		}
	}

}