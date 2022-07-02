using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using ArchimedesMiniGame;
using UnityEngine.SceneManagement;
using System;

[CreateAssetMenu(fileName ="ButtonInteractionSO", menuName ="SO/ButtonInteraction")]
public class ButtonInteractionScriptableObject : ScriptableObject
{
    public EInteractionType InteractionType;

    public void Interact(GameObject interestedObject, Interacter interacter, ButtonInteraction buttonInteractionHolder = null)
    {
        switch (InteractionType)
        {
            case EInteractionType.ZeroG:
                ZeroGInteraction();
                break;
            case EInteractionType.GoToCheckPoint:
                interacter.transform.position = interestedObject.transform.position;
                break;
            case EInteractionType.OpenDoor:
                OpenDoor(interestedObject, interacter, buttonInteractionHolder);
                break;
            case EInteractionType.ActiveModule:
                ActiveModule(interestedObject);
                break;
            case EInteractionType.PlayPacMan:
                PlayPacMan();
                break;
            case EInteractionType.RotateRoom:
                RotateRoom(interestedObject);
                break;
            case EInteractionType.OpenTerminal:
                OpenTerminal(interestedObject);
                break;
            case EInteractionType.Integration:
                Integration(interestedObject);
                break;
        }
    }

    private void Integration(GameObject interestedObject)
    {
        interestedObject.GetComponent<IntegrationEvents>().Invoke();
    }

    private void OpenTerminal(GameObject interestedObject)
    {
        interestedObject.GetComponent<Terminal>().ReadTerminal();
    }

    private void RotateRoom(GameObject interestedObject)
    {
        interestedObject.GetComponentInChildren<Rotation>().SetRotationDestination();
    }

    private void ZeroGInteraction()
    {
        PubSub.PubSub.Publish(new ZeroGMessage(!GameManager.Instance.ZeroGActive));
    }

    private void OpenDoor(GameObject interestedObject, Interacter interacter, ButtonInteraction buttonInteractionHolder)
    {
        interestedObject.GetComponentInChildren<Interactable>().Interact(interacter);
        interestedObject.GetComponentInChildren<MovableObject>().SetButtonActivator(buttonInteractionHolder);
    }
    
    private void ActiveModule(GameObject interestedObject)
    {
        interestedObject.GetComponent<Module>().TryStartEngine();
    }

    private void PlayPacMan()
    {
        PubSub.PubSub.Publish(new PlayPacManMessage());
    }

}
