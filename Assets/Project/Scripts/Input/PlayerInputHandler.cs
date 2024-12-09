//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Project/Prefabs/PlayerInputHandler.inputactions
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

public partial class @PlayerInputHandler: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputHandler()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputHandler"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""c9349f14-044d-4889-92aa-de7b6f428b54"",
            ""actions"": [
                {
                    ""name"": ""Choosed"",
                    ""type"": ""Button"",
                    ""id"": ""33701acd-a2a8-4ef0-a0d9-30bbd2c0078b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FlagSetted"",
                    ""type"": ""Button"",
                    ""id"": ""a45182fd-edb9-4bbc-ba8b-2565c41e234c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""76267b1a-1534-473e-8a60-af97da486dcd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Choosed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b06b8ff-54ab-4d93-b2ed-50974df3f836"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""FlagSetted"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Choosed = m_Player.FindAction("Choosed", throwIfNotFound: true);
        m_Player_FlagSetted = m_Player.FindAction("FlagSetted", throwIfNotFound: true);
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
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Choosed;
    private readonly InputAction m_Player_FlagSetted;
    public struct PlayerActions
    {
        private @PlayerInputHandler m_Wrapper;
        public PlayerActions(@PlayerInputHandler wrapper) { m_Wrapper = wrapper; }
        public InputAction @Choosed => m_Wrapper.m_Player_Choosed;
        public InputAction @FlagSetted => m_Wrapper.m_Player_FlagSetted;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Choosed.started += instance.OnChoosed;
            @Choosed.performed += instance.OnChoosed;
            @Choosed.canceled += instance.OnChoosed;
            @FlagSetted.started += instance.OnFlagSetted;
            @FlagSetted.performed += instance.OnFlagSetted;
            @FlagSetted.canceled += instance.OnFlagSetted;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Choosed.started -= instance.OnChoosed;
            @Choosed.performed -= instance.OnChoosed;
            @Choosed.canceled -= instance.OnChoosed;
            @FlagSetted.started -= instance.OnFlagSetted;
            @FlagSetted.performed -= instance.OnFlagSetted;
            @FlagSetted.canceled -= instance.OnFlagSetted;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnChoosed(InputAction.CallbackContext context);
        void OnFlagSetted(InputAction.CallbackContext context);
    }
}
