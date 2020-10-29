// GENERATED AUTOMATICALLY FROM 'Assets/Common/Input/GamepadControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GamepadControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @GamepadControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GamepadControls"",
    ""maps"": [
        {
            ""name"": ""gameplay"",
            ""id"": ""d037cf2c-5453-4c50-8506-b66efdbbf3e1"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""dc21bb32-67fb-48db-9646-5dd93115cdf2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""87bd9610-e51b-4b71-9ca2-2beaef1083f6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""1fb1297b-1351-4c93-aeee-4710918056f6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AttackL"",
                    ""type"": ""Button"",
                    ""id"": ""499aaf2f-0fd3-4953-8edb-9c6bad3a7518"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AttackR"",
                    ""type"": ""Button"",
                    ""id"": ""7fed3b43-da94-4784-b03b-b943117561bf"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KickL"",
                    ""type"": ""Button"",
                    ""id"": ""7474a8c7-0aa2-4bbc-8edc-b2a9d16713a5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KickR"",
                    ""type"": ""Button"",
                    ""id"": ""4b586d3a-849d-4f7e-b9bd-efe16a88a882"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraUp/Down"",
                    ""type"": ""Value"",
                    ""id"": ""544a359d-43b6-4523-a8e0-11adda77ff57"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraForward/Back"",
                    ""type"": ""Value"",
                    ""id"": ""6e29a367-8487-4ce6-8df9-115da61fd895"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NextTarget"",
                    ""type"": ""PassThrough"",
                    ""id"": ""31fcc513-8f82-42d9-9389-8d6316d01cda"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrevTarget"",
                    ""type"": ""Button"",
                    ""id"": ""82195c69-4df6-4743-b75c-76ab1ac16c4a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""1b557217-e440-496e-b681-5122cdcfe2f2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""2db5f0be-ef48-41d8-93af-204a079c9af0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""3cb61180-f422-40e8-9dd0-e274df1487d2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a00b2419-b5d5-423f-ac30-ae4d7df7a5fc"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Dpad"",
                    ""id"": ""ea686c37-6bac-4b88-b876-a441b70e299b"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""93f8e940-87f5-4305-bb2b-42dce0f9aa68"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e3b29276-dd63-4303-8f77-64f32e5156ac"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""29d6c791-2321-424c-a8a3-57c9afb87d95"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""273abca6-a19a-47cd-80d4-a4d2440d057c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9a4da707-6bf9-4486-8d6d-0d75148f0e96"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58722705-a7a2-4b51-b896-0bde38b449db"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""188af592-dba4-46fe-9508-2a39afdc9ada"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efe5aaea-f146-4af5-a44b-fc4ce31620ab"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7eb5dbf-8fd6-4512-82bf-775ff0893435"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43cc9b59-5d12-4a8b-8016-41a927bc741e"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a35a43be-c603-4060-9361-a101cc187cbb"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KickL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07fe92f8-26d6-475d-9af1-acc465ca93b4"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KickL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f99cf15-53ec-4988-9211-5535bda94b35"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KickR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b62fe75e-d000-4a0e-8dfd-15428c5ff069"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KickR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""021bdf8a-e59e-47ce-8704-864ef77d4554"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraUp/Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""+/-"",
                    ""id"": ""defec368-d002-498f-bb94-ac4a991b7e14"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraUp/Down"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""60ef62f3-a6c1-4bdd-bd50-8574617051c1"",
                    ""path"": ""<Keyboard>/minus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraUp/Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""f4027a3e-1aac-4901-be26-df82e1556029"",
                    ""path"": ""<Keyboard>/equals"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraUp/Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2f1f2263-bdda-468a-be82-62777e4be525"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraForward/Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""[]"",
                    ""id"": ""25f41efc-d190-4035-8790-1f5386d9a9c3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraForward/Back"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""16783a6a-31d2-40cb-8098-263dbf2a2f79"",
                    ""path"": ""<Keyboard>/leftBracket"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraForward/Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""080b951a-5fdc-4c86-9966-6e616d433821"",
                    ""path"": ""<Keyboard>/rightBracket"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraForward/Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c49a82e8-1a96-4e37-aafc-9d3f7b24ed3a"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7cb7688c-8603-4cfd-afdf-db427751ebc7"",
                    ""path"": ""<Keyboard>/rightAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5114f839-69db-4b86-822f-c4f5f2df7a85"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrevTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b3ef698-b49b-4009-aaff-a3377f827d33"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrevTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4b89c40-ddfb-41e8-bcf6-50e164f110ca"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9e161d8-a229-47c9-b30e-49b978455432"",
                    ""path"": ""<Keyboard>/rightShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0dc06bff-0120-44f6-b71a-dc0574c8c39b"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6b057d5-2f18-48f0-835a-d72589a58a12"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fd5a0a5-606f-4f5a-b3c7-a0f28b75956c"",
                    ""path"": ""<DualShockGamepad>/touchpadButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59fccb5e-df48-4937-a0e5-d3fee1bed9ae"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // gameplay
        m_gameplay = asset.FindActionMap("gameplay", throwIfNotFound: true);
        m_gameplay_Movement = m_gameplay.FindAction("Movement", throwIfNotFound: true);
        m_gameplay_Start = m_gameplay.FindAction("Start", throwIfNotFound: true);
        m_gameplay_Select = m_gameplay.FindAction("Select", throwIfNotFound: true);
        m_gameplay_AttackL = m_gameplay.FindAction("AttackL", throwIfNotFound: true);
        m_gameplay_AttackR = m_gameplay.FindAction("AttackR", throwIfNotFound: true);
        m_gameplay_KickL = m_gameplay.FindAction("KickL", throwIfNotFound: true);
        m_gameplay_KickR = m_gameplay.FindAction("KickR", throwIfNotFound: true);
        m_gameplay_CameraUpDown = m_gameplay.FindAction("CameraUp/Down", throwIfNotFound: true);
        m_gameplay_CameraForwardBack = m_gameplay.FindAction("CameraForward/Back", throwIfNotFound: true);
        m_gameplay_NextTarget = m_gameplay.FindAction("NextTarget", throwIfNotFound: true);
        m_gameplay_PrevTarget = m_gameplay.FindAction("PrevTarget", throwIfNotFound: true);
        m_gameplay_Roll = m_gameplay.FindAction("Roll", throwIfNotFound: true);
        m_gameplay_Block = m_gameplay.FindAction("Block", throwIfNotFound: true);
        m_gameplay_LightAttack = m_gameplay.FindAction("LightAttack", throwIfNotFound: true);
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

    // gameplay
    private readonly InputActionMap m_gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_gameplay_Movement;
    private readonly InputAction m_gameplay_Start;
    private readonly InputAction m_gameplay_Select;
    private readonly InputAction m_gameplay_AttackL;
    private readonly InputAction m_gameplay_AttackR;
    private readonly InputAction m_gameplay_KickL;
    private readonly InputAction m_gameplay_KickR;
    private readonly InputAction m_gameplay_CameraUpDown;
    private readonly InputAction m_gameplay_CameraForwardBack;
    private readonly InputAction m_gameplay_NextTarget;
    private readonly InputAction m_gameplay_PrevTarget;
    private readonly InputAction m_gameplay_Roll;
    private readonly InputAction m_gameplay_Block;
    private readonly InputAction m_gameplay_LightAttack;
    public struct GameplayActions
    {
        private @GamepadControls m_Wrapper;
        public GameplayActions(@GamepadControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_gameplay_Movement;
        public InputAction @Start => m_Wrapper.m_gameplay_Start;
        public InputAction @Select => m_Wrapper.m_gameplay_Select;
        public InputAction @AttackL => m_Wrapper.m_gameplay_AttackL;
        public InputAction @AttackR => m_Wrapper.m_gameplay_AttackR;
        public InputAction @KickL => m_Wrapper.m_gameplay_KickL;
        public InputAction @KickR => m_Wrapper.m_gameplay_KickR;
        public InputAction @CameraUpDown => m_Wrapper.m_gameplay_CameraUpDown;
        public InputAction @CameraForwardBack => m_Wrapper.m_gameplay_CameraForwardBack;
        public InputAction @NextTarget => m_Wrapper.m_gameplay_NextTarget;
        public InputAction @PrevTarget => m_Wrapper.m_gameplay_PrevTarget;
        public InputAction @Roll => m_Wrapper.m_gameplay_Roll;
        public InputAction @Block => m_Wrapper.m_gameplay_Block;
        public InputAction @LightAttack => m_Wrapper.m_gameplay_LightAttack;
        public InputActionMap Get() { return m_Wrapper.m_gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Start.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Select.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSelect;
                @AttackL.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttackL;
                @AttackL.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttackL;
                @AttackL.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttackL;
                @AttackR.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttackR;
                @AttackR.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttackR;
                @AttackR.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttackR;
                @KickL.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKickL;
                @KickL.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKickL;
                @KickL.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKickL;
                @KickR.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKickR;
                @KickR.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKickR;
                @KickR.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKickR;
                @CameraUpDown.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraUpDown;
                @CameraUpDown.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraUpDown;
                @CameraUpDown.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraUpDown;
                @CameraForwardBack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraForwardBack;
                @CameraForwardBack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraForwardBack;
                @CameraForwardBack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraForwardBack;
                @NextTarget.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextTarget;
                @NextTarget.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextTarget;
                @NextTarget.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextTarget;
                @PrevTarget.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrevTarget;
                @PrevTarget.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrevTarget;
                @PrevTarget.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrevTarget;
                @Roll.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                @Block.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Block.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Block.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @LightAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLightAttack;
                @LightAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLightAttack;
                @LightAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLightAttack;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @AttackL.started += instance.OnAttackL;
                @AttackL.performed += instance.OnAttackL;
                @AttackL.canceled += instance.OnAttackL;
                @AttackR.started += instance.OnAttackR;
                @AttackR.performed += instance.OnAttackR;
                @AttackR.canceled += instance.OnAttackR;
                @KickL.started += instance.OnKickL;
                @KickL.performed += instance.OnKickL;
                @KickL.canceled += instance.OnKickL;
                @KickR.started += instance.OnKickR;
                @KickR.performed += instance.OnKickR;
                @KickR.canceled += instance.OnKickR;
                @CameraUpDown.started += instance.OnCameraUpDown;
                @CameraUpDown.performed += instance.OnCameraUpDown;
                @CameraUpDown.canceled += instance.OnCameraUpDown;
                @CameraForwardBack.started += instance.OnCameraForwardBack;
                @CameraForwardBack.performed += instance.OnCameraForwardBack;
                @CameraForwardBack.canceled += instance.OnCameraForwardBack;
                @NextTarget.started += instance.OnNextTarget;
                @NextTarget.performed += instance.OnNextTarget;
                @NextTarget.canceled += instance.OnNextTarget;
                @PrevTarget.started += instance.OnPrevTarget;
                @PrevTarget.performed += instance.OnPrevTarget;
                @PrevTarget.canceled += instance.OnPrevTarget;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @Block.started += instance.OnBlock;
                @Block.performed += instance.OnBlock;
                @Block.canceled += instance.OnBlock;
                @LightAttack.started += instance.OnLightAttack;
                @LightAttack.performed += instance.OnLightAttack;
                @LightAttack.canceled += instance.OnLightAttack;
            }
        }
    }
    public GameplayActions @gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnAttackL(InputAction.CallbackContext context);
        void OnAttackR(InputAction.CallbackContext context);
        void OnKickL(InputAction.CallbackContext context);
        void OnKickR(InputAction.CallbackContext context);
        void OnCameraUpDown(InputAction.CallbackContext context);
        void OnCameraForwardBack(InputAction.CallbackContext context);
        void OnNextTarget(InputAction.CallbackContext context);
        void OnPrevTarget(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnLightAttack(InputAction.CallbackContext context);
    }
}
