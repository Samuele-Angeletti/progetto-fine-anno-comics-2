using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using TMPro;

public class UIPauseMenu : Menu
{
	[SerializeField] TextMeshProUGUI m_QuestText;

	public override void Open()
	{
		base.Open();
		PubSub.PubSub.Publish(new PauseGameMessage());
	}

	public override void Close()
	{
		base.Close();
		PubSub.PubSub.Publish(new ResumeGameMessage());
	}

    public override void Hide()
    {
		base.Close();
    }

    public override void Show()
    {
		base.Open();
	}
	public void SetQuestText(string text)
    {
		m_QuestText.text = text;
    }
	
}