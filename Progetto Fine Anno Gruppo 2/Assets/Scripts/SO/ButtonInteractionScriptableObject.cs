using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using ArchimedesMiniGame;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName ="ButtonInteractionSO", menuName ="ButtonInteraction")]
public class ButtonInteractionScriptableObject : ScriptableObject
{
    public EInteractionType InteractionType;

    public void Interact(GameObject interestedObject, Interacter interacter)
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
                OpenDoor(interestedObject, interacter);
                break;
            case EInteractionType.ActiveModule:
                ActiveModule(interestedObject);
                break;
            case EInteractionType.PlayPacMan:
                PlayPacMan();
                break;
        }
    }

    private void ZeroGInteraction()
    {
        PubSub.PubSub.Publish(new ZeroGMessage(!GameManagerIN.Instance.ZeroGActive));
    }

    private void OpenDoor(GameObject interestedObject, Interacter interacter)
    {
        interestedObject.GetComponentInChildren<Interactable>().Interact(interacter);
    }
    
    private void ActiveModule(GameObject interestedObject)
    {
        interestedObject.GetComponent<Module>().StartEngine();
    }

    private void PlayPacMan()
    {
        SceneManager.LoadScene("PacMan");
    }
}
