using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;

public class ButtonInteraction : Interactable
{

    public override void Interact(Interacter interacter)
    {
        switch (InteractionType)
        {
            case EInteractionType.ZeroG:
                Debug.LogError("Per la ZeroG devi usare lo script ZeroGInteraction");
                break;
            case EInteractionType.GoToCheckPoint:
                interacter.transform.position = InterestedObject.transform.position;
                break;
            case EInteractionType.Action:
                InterestedObject.GetComponentInChildren<Interactable>().Interact(interacter);
                break;
        }
    }

    public override void ShowUI(bool isVisible)
    {

    }

}
