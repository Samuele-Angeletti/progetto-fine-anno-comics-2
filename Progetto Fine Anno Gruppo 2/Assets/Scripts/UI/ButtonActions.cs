using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using System;
using UnityEngine.SceneManagement;
using MainGame;
using ArchimedesMiniGame;
using MicroGame;

public class ButtonActions : MonoBehaviour
{
	[SerializeField] EButtonType m_ButtonType;
	[SerializeField] EMenu m_MenuToOpen;
    [SerializeField] string m_GoToScene;
    [SerializeField] TerminalScriptableObject m_StoredTerminalToRead;

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
            case EButtonType.ForceResume:
                PubSub.PubSub.Publish(new ResumeGameMessage());
                break;
            case EButtonType.ReadStoredTerminal:
                PubSub.PubSub.Publish(new TerminalMessage(m_StoredTerminalToRead));
                break;
            case EButtonType.ForceGoToScene:
                ForceLoadToScene();
                break;
        }
    }

    private void GoToScene()
    {
        PubSub.PubSub.Publish(new GoToSceneMessage(m_GoToScene));
    }

    private void ForceLoadToScene()
    {
        Destroy(UIManager.Instance.gameObject);
        SceneManager.UnloadSceneAsync("TestPlatformPuzzle1");
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

    public void SetTerminalSO(TerminalScriptableObject terminalScriptableObject)
    {
        m_StoredTerminalToRead = terminalScriptableObject;
    }
}
