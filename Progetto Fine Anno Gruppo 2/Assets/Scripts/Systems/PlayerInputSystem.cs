using System;
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

            inputControls.Player.Interaction.performed += Interact;
            inputControls.Player.ZeroG.started += ZeroGDebug;

            inputControls.Module.Rotation.performed += StartRotation;
            inputControls.Module.Rotation.canceled += StopRotation;

            inputControls.Player.Pause.performed += PauseGame;
        }

        private void ZeroGDebug(InputAction.CallbackContext obj)
        {
            if (!DialoguePlayer.Instance.dialogueIsPlaying)
                m_CurrentControllable.DebugZeroG();
        }

        private void Interact(InputAction.CallbackContext obj)
        {
            if (!DialoguePlayer.Instance.dialogueIsPlaying)
                m_CurrentControllable.Interact();

        }

        private void PauseGame(InputAction.CallbackContext obj)
        {
            PubSub.PubSub.Publish(new OpenMenuMessage(EMenu.Pause));
        }

        private void StopRotation(InputAction.CallbackContext obj)
        {
            if (!DialoguePlayer.Instance.dialogueIsPlaying)
                m_CurrentControllable.MoveRotation(obj.ReadValue<Vector2>());
        }

        private void StartRotation(InputAction.CallbackContext obj)
        {
            if (!DialoguePlayer.Instance.dialogueIsPlaying)
                m_CurrentControllable.MoveRotation(obj.ReadValue<Vector2>());
        }

        private void StartMoving(InputAction.CallbackContext obj)
        {
            if (!DialoguePlayer.Instance.dialogueIsPlaying)
                m_CurrentControllable.MoveDirection(obj.ReadValue<Vector2>());
        }

        private void StopMoving(InputAction.CallbackContext obj)
        {
            if (!DialoguePlayer.Instance.dialogueIsPlaying)
                m_CurrentControllable.MoveDirection(obj.ReadValue<Vector2>());
        }

        private void OnDestroy()
        {
            DeactiveMovement();

            inputControls.Module.Rotation.performed -= StartRotation;
            inputControls.Module.Rotation.canceled -= StopRotation;

            inputControls.Player.Pause.performed -= PauseGame;
        }

        public void DeactiveMovement()
        {
            inputControls.Player.MovementWASD.performed -= StartMoving;
            inputControls.Player.MovementWASD.canceled -= StopMoving;


            inputControls.Player.MovementArrows.performed -= StartMoving;
            inputControls.Player.MovementArrows.canceled -= StopMoving;
        }


        private void StopJump(InputAction.CallbackContext obj)
        {
            if (!DialoguePlayer.Instance.dialogueIsPlaying)
                m_CurrentControllable.Jump(false);
        }

        private void StartJump(InputAction.CallbackContext obj)
        {
            if (!DialoguePlayer.Instance.dialogueIsPlaying)
            {
                if(obj.phase == InputActionPhase.Performed)
                    m_CurrentControllable.Jump(true);
            }
        }

        private void ActiveContinousMovement()
        {
            inputControls.Player.MovementArrows.started += StartMoving;
            inputControls.Player.MovementArrows.canceled += StopMoving;

            inputControls.Player.MovementWASD.started += StartMoving;
            inputControls.Player.MovementWASD.canceled += StopMoving;
        }

        private void ActiveNormalMovement()
        {
            inputControls.Player.MovementWASD.performed += StartMoving;
            inputControls.Player.MovementWASD.canceled += StopMoving;

            inputControls.Player.MovementArrows.performed += StartMoving;
            inputControls.Player.MovementArrows.canceled += StopMoving;

            inputControls.Player.Jump.performed += StartJump;
            inputControls.Player.Jump.canceled += StopJump;
        }


        #endregion

        private Controllable m_CurrentControllable;

        public void SetControllable(Controllable controllable)
        {
            m_CurrentControllable = controllable;

            ChangeContinousMovement(m_CurrentControllable.ContinousMovement);
        }

        public void ChangeContinousMovement(bool active)
        {
            DeactiveMovement();
            if (active)
            {
                ActiveContinousMovement();
            }
            else
            {
                ActiveNormalMovement();
            }
        }
    }
}
