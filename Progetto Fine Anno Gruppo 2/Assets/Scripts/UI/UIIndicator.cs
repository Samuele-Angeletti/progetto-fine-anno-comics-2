using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIIndicator : MonoBehaviour
{
	[Header("Scene References")]
	[SerializeField] Transform m_OriginPoint;
	[SerializeField] Transform m_DestinationPoint;
	[SerializeField] GameObject m_ImagePrefab;

	[Header("UI References")]
	[SerializeField] GameObject m_Indicator;
	[SerializeField] TextMeshProUGUI m_MetersText;

	private bool m_Active;
	private bool m_Spawned;
	void Start()
	{
	}

	void Update()
	{
		if (m_Active)
		{
			UpdateRotation();
			UpdatePosition();
			UpdateMeters();
		}
	}

	private void UpdateRotation()
	{
		Vector3 direction = m_DestinationPoint.position - m_OriginPoint.position;
		m_Indicator.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
	}

	private void UpdatePosition()
	{

	}

	private void UpdateMeters()
    {
		int meters = (int)Vector3.Distance(m_DestinationPoint.position, m_OriginPoint.position);
		string text;
		if (meters >= 10000 && !m_Spawned)
		{
			Instantiate(m_ImagePrefab, Vector3.zero, Quaternion.identity);
			m_Spawned = true;
		}
		if (meters > 1000)
		{
			meters /= 1000;
			text = meters + " km";
		}
		else
		{
			text = meters + " m";
		}
		m_MetersText.text = text;
    }

	public void ActiveIndicator(bool active, Transform origin, Transform destination)
	{
		m_Indicator.SetActive(active);
		m_Active = active;
		m_MetersText.gameObject.SetActive(active);
		m_OriginPoint = origin;
		m_DestinationPoint = destination;
	}

	
}
