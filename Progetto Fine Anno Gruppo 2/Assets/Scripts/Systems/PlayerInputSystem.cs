using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Commons
{
    public class PlayerInputSystem : MonoBehaviour
    {
        #region INPUTS
        public InputControls inputControls;

        private void Awake()
        {
            inputControls = new InputControls();

            inputControls.Player.Enable();
            inputControls.Module.Enable();

            ActiveNormalMovement();

            inputControls.Module.Rotation.performed += StartRotation;
            inputControls.Module.Rotation.canceled += StopRotation;

            inputControls.Player.Pause.performed += PauseGame;
        }

        private void PauseGame(InputAction.CallbackContext obj)
        {
            PubSub.PubSub.Publish(new OpenMenuMessage(EMenu.Pause));
        }

        private void StopRotation(InputAction.CallbackContext obj)
        {
            m_CurrentControllable.MoveRotation(obj.ReadValue<Vector2>());
        }

        private void StartRotation(InputAction.CallbackContext obj)
        {
            m_CurrentControllable.MoveRotation(obj.ReadValue<Vector2>());
        }

        private void StartMoving(InputAction.CallbackContext obj)
        {
            m_CurrentControllable.MoveDirection(obj.ReadValue<Vector2>());
        }

        private void StopMoving(InputAction.CallbackContext obj)
        {
            m_CurrentControllable.MoveDirection(obj.ReadValue<Vector2>());
        }

        private void OnDestroy()
        {
            DeactiveMovement();

            inputControls.Module.Rotation.performed -= StartRotation;
            inputControls.Module.Rotation.canceled -= StopRotation;

            inputControls.Player.Pause.performed -= PauseGame;
        }

        private void DeactiveMovement()
        {
            inputControls.Player.MovementWASD.performed -= StartMoving;
            //inputControls.Player.MovementArrows.performed -= StartMoving;
            inputControls.Player.MovementWASD.canceled -= StopMoving;
            //inputControls.Player.MovementArrows.canceled -= StopMoving;
        }
        #endregion

        private Controllable m_CurrentControllable;

        public void SetControllable(Controllable controllable)
        {
            m_CurrentControllable = controllable;

            DeactiveMovement();

            if (m_CurrentControllable.ContinousMovement)
            {
                ActiveContinousMovement();
            }
            else
            {
                ActiveNormalMovement();
            }
        }

        private void ActiveContinousMovement()
        {
            //inputControls.Player.MovementArrows.started += StartMoving;
            inputControls.Player.MovementWASD.started += StartMoving;
        }

        private void ActiveNormalMovement()
        {
            //inputControls.Player.MovementArrows.performed += StartMoving;
            inputControls.Player.MovementWASD.performed += StartMoving;
            //inputControls.Player.MovementArrows.canceled += StopMoving;
            inputControls.Player.MovementWASD.canceled += StopMoving;
        }
    }
}
