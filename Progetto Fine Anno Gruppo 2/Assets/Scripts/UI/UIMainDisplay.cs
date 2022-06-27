using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using MicroGame;
using UnityEngine.Events;

public class UIMainDisplay : MonoBehaviour, ISubscriber
{
    [SerializeField] GameObject m_DialogueBackground;
    [SerializeField] GameObject m_TerminalBackground;
    [SerializeField] GameObject m_LoadingInterface;
    [SerializeField] Image m_ProgressBar;
    [SerializeField] GameObject m_MainMenu;
    [SerializeField] GameObject m_MainMenuBackGround;
    [SerializeField] UnityEvent m_EventOnEndLoad;
    [SerializeField] List<GameObject> m_DestroyOnLoad;
    bool m_GoigToMainMenu;
    bool m_GoingToPacMan;
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(PlayPacManMessage));
        PubSub.PubSub.Subscribe(this, typeof(GoToSceneMessage));
    }
    public void LoadNewSceneASync(string sceneToLoad)
    {
        ActiveLoadingBackground(true);
        scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());
    }

    private IEnumerator LoadingScreen()
    {
        float totalProgress = 0;
        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while(!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                m_ProgressBar.fillAmount = totalProgress / scenesToLoad.Count;
                yield return null;
            }
        }

        if(m_GoingToPacMan)
        {
            UIManager.Instance.OpenPacManInterface(true);
            m_GoingToPacMan = false;
        }
        else if (m_GoigToMainMenu)
        {
            m_GoigToMainMenu = false;
            ActiveMainMenu(true);
        }
        else
        {
            ActiveMainMenu(false);
        }
        DestroyObjects();
        ActiveLoadingBackground(false);
    }

    private void DestroyObjects()
    {
        foreach(GameObject g in m_DestroyOnLoad)
        {
            g.SetActive(false);
            Destroy(g, 3f);
        }
    }
    
    public void ActiveLoadingBackground(bool active)
    {
        m_LoadingInterface.SetActive(active);        
    }

    public void ActiveDialogueBackground(bool active)
    {
        m_DialogueBackground.SetActive(active);
    }

    public void ActiveTerminalBackground(bool active)
    {
        m_TerminalBackground.SetActive(active);
    }

    public void OnPublish(IMessage message)
    {
        if(message is PlayPacManMessage)
        {
            LoadNewSceneASync("PacMan");
            m_GoingToPacMan = true;
        }
        else if(message is GoToSceneMessage)
        {
            GoToSceneMessage goToSceneMessage = (GoToSceneMessage)message;
            if (goToSceneMessage.SceneName == "MainMenu") m_GoigToMainMenu = true;
            LoadNewSceneASync(goToSceneMessage.SceneName);
        }
    }

    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(PlayPacManMessage));
    }

    private void OnDestroy()
    {
        OnDisableSubscribe();
    }

    public void ActiveMainMenu(bool active)
    {
        m_MainMenu.SetActive(active);
        m_MainMenuBackGround.SetActive(active);
    }
}