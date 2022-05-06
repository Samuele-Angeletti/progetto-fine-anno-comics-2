using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace ArchimedesMiniGame
{
    public class PlayerController : MonoBehaviour
    {
        #region INPUTS
        public InputControls inputControls;

        private void Awake()
        {
            inputControls = new InputControls();

            inputControls.Player.Enable();

            inputControls.Player.Movement.performed += StartMoving;
            inputControls.Player.Movement.canceled += StopMoving;

            inputControls.Player.Rotation.performed += StartRotation;
            inputControls.Player.Rotation.canceled += StopRotation;

            inputControls.Player.Pause.performed += PauseGame;
        }

        private void PauseGame(InputAction.CallbackContext obj)
        {
            PubSub.PubSub.Publish(new OpenMenuMessage(EMenu.Pause));
        }

        private void StopRotation(InputAction.CallbackContext obj)
        {
            m_Module.Rotate(Vector2.zero);
        }

        private void StartRotation(InputAction.CallbackContext obj)
        {
            m_Module.Rotate(obj.ReadValue<Vector2>());
        }

        private void StartMoving(InputAction.CallbackContext obj)
        {
            m_Module.AddForce(obj.ReadValue<Vector2>());
        }

        private void StopMoving(InputAction.CallbackContext obj)
        {
            m_Module.AddForce(obj.ReadValue<Vector2>());
        }

        private void OnDestroy()
        {
            inputControls.Player.Movement.performed -= StartMoving;
            inputControls.Player.Movement.canceled -= StopMoving;

            inputControls.Player.Rotation.performed -= StartRotation;
            inputControls.Player.Rotation.canceled -= StopRotation;

            inputControls.Player.Pause.performed -= PauseGame;
        }

        #endregion

        [SerializeField] Module m_Module;

    }
}
