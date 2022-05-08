//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputSystem/InputControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""f6ea4f1c-46f8-4098-945a-2b0cacea15e3"",
            ""actions"": [
                {
                    ""name"": ""MovementWASD"",
                    ""type"": ""Value"",
                    ""id"": ""f192a576-4167-4a75-b8ec-8b2478111374"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MovementArrows"",
                    ""type"": ""Value"",
                    ""id"": ""0c558c9c-a13a-40b9-8d9b-12cea1a2eb25"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""bdb8eaf2-9ccc-4ae2-b908-49ba7a8783d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""8de74b2e-3ad9-45ed-89c3-133c62aadd9f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""4c46edaa-942d-4d99-a8c3-d9d165736340"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c970f34d-fdeb-4dca-9061-29948b3d367a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e793362b-57a0-44e6-91ca-b4c306c5bec8"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f831ea36-89a0-413d-9c6a-97c7c8b15811"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""054aa46e-0b96-4919-bb15-ffa7ed721ff7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementWASD"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""995dd93f-b6ef-437c-988e-9e048220e175"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementWASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""292cefe3-ee2d-487d-a94e-9fe425b25403"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementWASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5e068c7a-8bc5-46fc-9026-aa068abb9c00"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementWASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fb13a567-03ac-47bb-b22b-9856365fba28"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementWASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f46956dc-8270-49e8-8419-315dcbdd4fff"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementArrows"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""894c2e7a-5495-4b3d-92e6-907e87fbcd7f"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementArrows"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e260f13d-17fd-4f72-9cae-a98063bcd4fe"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementArrows"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""68867c59-09af-48e9-a10d-5e8ade364027"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementArrows"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9bb74fe0-de9e-4693-8e3e-36f5eacbc14b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementArrows"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Module"",
            ""id"": ""29cda60e-8fa3-4452-8b33-b8cde328f243"",
            ""actions"": [
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""3b0e022f-35bf-4683-b1c6-bf55c14a7c41"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5be96e20-c94e-4d4b-a256-f69088d8c33d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0f36a19d-edc7-46ff-aa10-53204b5c88a2"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""195364f4-0231-4d82-91cd-248ab155c750"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MovementWASD = m_Player.FindAction("MovementWASD", throwIfNotFound: true);
        m_Player_MovementArrows = m_Player.FindAction("MovementArrows", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
        m_Player_Interaction = m_Player.FindAction("Interaction", throwIfNotFound: true);
        // Module
        m_Module = asset.FindActionMap("Module", throwIfNotFound: true);
        m_Module_Rotation = m_Module.FindAction("Rotation", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_MovementWASD;
    private readonly InputAction m_Player_MovementArrows;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Pause;
    private readonly InputAction m_Player_Interaction;
    public struct PlayerActions
    {
        private @InputControls m_Wrapper;
        public PlayerActions(@InputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MovementWASD => m_Wrapper.m_Player_MovementWASD;
        public InputAction @MovementArrows => m_Wrapper.m_Player_MovementArrows;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputAction @Interaction => m_Wrapper.m_Player_Interaction;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MovementWASD.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementWASD;
                @MovementWASD.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementWASD;
                @MovementWASD.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementWASD;
                @MovementArrows.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementArrows;
                @MovementArrows.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementArrows;
                @MovementArrows.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementArrows;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Interaction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteraction;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MovementWASD.started += instance.OnMovementWASD;
                @MovementWASD.performed += instance.OnMovementWASD;
                @MovementWASD.canceled += instance.OnMovementWASD;
                @MovementArrows.started += instance.OnMovementArrows;
                @MovementArrows.performed += instance.OnMovementArrows;
                @MovementArrows.canceled += instance.OnMovementArrows;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Module
    private readonly InputActionMap m_Module;
    private IModuleActions m_ModuleActionsCallbackInterface;
    private readonly InputAction m_Module_Rotation;
    public struct ModuleActions
    {
        private @InputControls m_Wrapper;
        public ModuleActions(@InputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotation => m_Wrapper.m_Module_Rotation;
        public InputActionMap Get() { return m_Wrapper.m_Module; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ModuleActions set) { return set.Get(); }
        public void SetCallbacks(IModuleActions instance)
        {
            if (m_Wrapper.m_ModuleActionsCallbackInterface != null)
            {
                @Rotation.started -= m_Wrapper.m_ModuleActionsCallbackInterface.OnRotation;
                @Rotation.performed -= m_Wrapper.m_ModuleActionsCallbackInterface.OnRotation;
                @Rotation.canceled -= m_Wrapper.m_ModuleActionsCallbackInterface.OnRotation;
            }
            m_Wrapper.m_ModuleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rotation.started += instance.OnRotation;
                @Rotation.performed += instance.OnRotation;
                @Rotation.canceled += instance.OnRotation;
            }
        }
    }
    public ModuleActions @Module => new ModuleActions(this);
    public interface IPlayerActions
    {
        void OnMovementWASD(InputAction.CallbackContext context);
        void OnMovementArrows(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
    }
    public interface IModuleActions
    {
        void OnRotation(InputAction.CallbackContext context);
    }
}
