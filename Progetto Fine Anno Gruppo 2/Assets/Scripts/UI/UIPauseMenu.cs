using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class UIPauseMenu : Menu
{
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
}