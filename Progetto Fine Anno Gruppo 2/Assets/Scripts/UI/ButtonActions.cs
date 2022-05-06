using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using System;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
	[SerializeField] EButtonType m_ButtonType;
	[SerializeField] EMenu m_MenuToOpen;
    [SerializeField] string m_GoToScene;

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
            case EButtonType.GoToScene:
                GoToScene();
                break;
        }
    }

    private void GoToScene()
    {
        SceneManager.LoadScene(m_GoToScene);
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
