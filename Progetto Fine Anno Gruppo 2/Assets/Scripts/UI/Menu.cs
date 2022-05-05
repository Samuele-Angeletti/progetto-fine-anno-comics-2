using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class Menu : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] Animator m_Animator;
	[Tooltip("If this is empity it will play the default opening with trigger PlayOpening. Else you can place the string you like to play a different animation")]
	[SerializeField] string m_OpeningTrigger = "PlayOpening";
	[Tooltip("If this is empity it will play the default opening with trigger PlayClosing. Else you can place the string you like to play a different animation")]
	[SerializeField] string m_ClosingTrigger = "PlayClosing";

	public virtual void Open()
	{
		if (m_Animator != null)
		{
			m_OpeningTrigger = m_OpeningTrigger == "" ? "PlayOpening" : m_OpeningTrigger;

			m_Animator.SetTrigger(m_OpeningTrigger);
		}
	}

	public virtual void Close()
	{
		if (m_Animator != null)
		{
			m_ClosingTrigger = m_ClosingTrigger == "" ? "PlayClosing" : m_ClosingTrigger;

			m_Animator.SetTrigger(m_ClosingTrigger);
		}
	}
}