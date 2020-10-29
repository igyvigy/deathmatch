using UnityEngine;
using System.Collections;

namespace DeathMatchCharacterAnims
{

    public class DeathMatchPlayerInputController : MonoBehaviour
    {
        private GamepadInputManager inputManager;
        //Inputs.
        [HideInInspector] public bool inputJump;
        [HideInInspector] public bool inputLightHit;
        [HideInInspector] public bool inputDeath;
        [HideInInspector] public bool inputAttackL;
        [HideInInspector] public bool inputAttackR;
        [HideInInspector] public bool inputSwitchUpDown;
        [HideInInspector] public bool inputNextTarget;
        private bool? lastInputNextTarget;
        [HideInInspector] public bool inputPrevTarget;
        private bool? lastInputPrevTarget;
        [HideInInspector] public bool inputBlock;
        [HideInInspector] public bool inputStrafe;
        [HideInInspector] public float inputAimVertical = 0;
        [HideInInspector] public float inputAimHorizontal = 0;
        [HideInInspector] public float inputHorizontal = 0;
        [HideInInspector] public float inputVertical = 0;
        [HideInInspector] public bool inputRoll;

        //Variables
        [HideInInspector] public bool allowedInput = true;
        [HideInInspector] public Vector3 moveInput;
        [HideInInspector] public Vector2 aimInput;

        /// <summary>
        /// Input abstraction for easier asset updates using outside control schemes.
        /// </summary>
        void Inputs()
        {
            // inputJump = Input.GetButtonDown("Jump");
            inputLightHit = inputManager.LightAttackValue;
            inputAttackL = inputManager.AttackLValue;
            inputAttackR = inputManager.AttackRValue;
            // inputSwitchUpDown = inputManager.LightAttackValue;

            inputNextTarget = inputManager.NextTargetValue;
            inputPrevTarget = inputManager.PrevTargetValue;
            // inputBlock = inputManager.BlockValue;
            inputStrafe = inputManager.BlockValue;
            // inputAimVertical = Input.GetAxisRaw("AimVertical");
            // inputAimHorizontal = Input.GetAxisRaw("AimHorizontal");
            inputHorizontal = inputManager.MovementValue.x;
            inputVertical = inputManager.MovementValue.y;
            inputRoll = inputManager.RollValue;
        }

        void Awake()
        {
            inputManager = TagResolver.i.inputManager;
            allowedInput = true;
        }

        void Update()
        {
            Inputs();
            moveInput = CameraRelativeInput(inputHorizontal, inputVertical);
            aimInput = new Vector2(inputAimHorizontal, inputAimVertical);
        }

        /// <summary>
        /// Movement based off camera facing.
        /// </summary>
        Vector3 CameraRelativeInput(float inputX, float inputZ)
        {
            //Forward vector relative to the camera along the x-z plane   
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;
            //Right vector relative to the camera always orthogonal to the forward vector.
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            Vector3 relativeVelocity = inputHorizontal * right + inputVertical * forward;
            //Reduce input for diagonal movement.
            if (relativeVelocity.magnitude > 1)
            {
                relativeVelocity.Normalize();
            }
            return relativeVelocity;
        }

        public bool HasAnyInput()
        {
            if (allowedInput && moveInput != Vector3.zero && aimInput != Vector2.zero && inputJump != false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasMoveInput()
        {
            if (allowedInput && moveInput != Vector3.zero)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasAimInput()
        {
            if (allowedInput && (aimInput.x < -0.8f || aimInput.x > 0.8f) || (aimInput.y < -0.8f || aimInput.y > 0.8f))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}