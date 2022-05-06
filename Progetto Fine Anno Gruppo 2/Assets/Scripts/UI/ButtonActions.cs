using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using System;

public class ButtonActions : MonoBehaviour
{
	[SerializeField] EButtonType m_ButtonType;
	[SerializeField] EMenu m_MenuToOpen;

	public void Action()
	{
        switch (m_ButtonType)
        {
            case EButtonType.OpenMenu:
                OpenMenu();
                break;
            case EButtonType.QuitGame:
                QuitGame();
                break;
            case EButtonType.CloaseAllMenu:
                CloseAllMenus();
                break;
        }
    }

    private void CloseAllMenus()
    {
        PubSub.PubSub.Publish(new CloseAllMenusMessage());
    }

    private void OpenMenu()
	{
		PubSub.PubSub.Publish(new OpenMenuMessage(m_MenuToOpen));
	}

	private void QuitGame()
	{
		Application.Quit();
	}
}
