using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIMainDisplay : MonoBehaviour, ISubscriber
{
    [SerializeField] GameObject m_DialogueBackground;
    [SerializeField] GameObject m_TerminalBackground;
    [SerializeField] GameObject m_LoadingInterface;
    [SerializeField] Image m_ProgressBar;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(PlayPacManMessage));
    }
    public void LoadNewSceneASync()
    {
        ActiveLoadingBackground(true);
        scenesToLoad.Add(SceneManager.LoadSceneAsync("PacMan", LoadSceneMode.Additive));
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
            LoadNewSceneASync();
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
}