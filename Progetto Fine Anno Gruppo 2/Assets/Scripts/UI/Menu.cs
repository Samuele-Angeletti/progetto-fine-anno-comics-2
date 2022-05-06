using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class Menu : MonoBehaviour
{
	[SerializeField] GameObject m_Background;


	public virtual void Open()
	{
		m_Background.SetActive(true);
	}

	public virtual void Close()
	{
		m_Background.SetActive(false);
	}

	public virtual void Hide()
    {

    }

	public virtual void Show()
    {

    }
}