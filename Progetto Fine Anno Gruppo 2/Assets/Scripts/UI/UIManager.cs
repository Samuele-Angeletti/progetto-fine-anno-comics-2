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

	void Start()
	{
		PubSub.PubSub.Subscribe(this, typeof(OpenMenuMessage));
	}

	public void OnPublish(IMessage message)
	{
		if (message is OpenMenuMessage)
		{
			OpenMenuMessage openMenu = (OpenMenuMessage)message;
			OpenMenu(openMenu.MenuType);
		}
	}

	private void OpenMenu(EMenu menuToOpen)
	{
		if(m_CurrentMenu == GetMenuByEnum(menuToOpen)) // se apro lo stesso, chiudo lo stesso menu
        {
			m_CurrentMenu.Close();
			m_CurrentMenu = null;
			return;
		}

		if (m_CurrentMenu != null && menuToOpen != EMenu.ConfirmChoise) m_CurrentMenu.Close();

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