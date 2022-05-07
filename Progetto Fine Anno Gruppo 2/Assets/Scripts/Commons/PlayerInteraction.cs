using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    InputControls inputControls;
    private void Awake()
    {
        inputControls = new InputControls();
    }
    private void OnEnable()
    {
        inputControls.Player.Interaction.Enable();
    }
    private void OnDisable()
    {
        inputControls.Player.Interaction.Disable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.ShowUI(true);
            if (inputControls.Player.Interaction.ReadValue<float>() == 1)
            {
                interactable.Interact();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.ShowUI(false);
            interactable = null;
        }
        
    }
}
