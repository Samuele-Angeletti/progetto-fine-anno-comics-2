using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;
using MainGame;
public class ButtonInteraction : Interactable
{
    [Header("Button Interaction Settings")]
    [SerializeField] bool m_Active = true;
    
    public override void Interact(Interacter interacter)
    {
        if(m_Active)
            GameManager.Instance.GetButtonInteractionSO(InteractionType).Interact(InterestedObject, interacter);
    }

    public override void ShowUI(bool isVisible)
    {

    }

    public void SetActive(bool active)
    {
        m_Active = active;
    }
}
