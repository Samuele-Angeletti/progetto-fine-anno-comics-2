using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    [SerializeField] Transform m_EndPivot;
    [SerializeField] float m_TransitionSpeed;
    private Vector3 m_StartPivot;
    private bool m_Active;

    private void Awake()
    {
        m_StartPivot = transform.position;
    }

    public override void Interact(Interacter interacter)
    {
        m_Active = true;
    }

    private void Update()
    {
        if(m_Active)
        {
            transform.position = Vector3.Lerp(transform.position, m_EndPivot.position, m_TransitionSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, m_EndPivot.position) < 0.1f)
            {
                m_Active = false;
                transform.position = m_EndPivot.position;
                m_EndPivot.position = m_StartPivot;
                m_StartPivot = transform.position;
            }
        }
    }

    public override void ShowUI(bool isVisible)
    {
    }
}
