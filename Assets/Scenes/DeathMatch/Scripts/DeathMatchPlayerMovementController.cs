using UnityEngine;
using System.Collections;

namespace DeathMatchCharacterAnims
{
    public enum RPGCharacterStateFREE
    {
        Idle = 0,
        Move = 1,
        Jump = 2,
        DoubleJump = 3,
        Fall = 4,
        Block = 6,
        Roll = 8
    }

    public class DeathMatchPlayerMovementController : SuperStateMachine
    {
        private SuperCharacterController superCharacterController;
        private DeathMatchPlayerController playerController;
        public UnityEngine.AI.NavMeshAgent navMeshAgent;
        private DeathMatchPlayerInputController deathMatchPlayerInputController;
        private Rigidbody rb;
        private Animator animator;
        public RPGCharacterStateFREE rpgCharacterState;
        public bool useMeshNav = false;
        [HideInInspector] public Vector3 lookDirection { get; private set; }
        [HideInInspector] public bool isKnockback;
        public float knockbackMultiplier = 1f;

        //Jumping.
        [HideInInspector] public bool canJump;
        [HideInInspector] public bool canDoubleJump = false;
        bool doublejumped = false;
        public float gravity = 25.0f;
        public float jumpAcceleration = 5.0f;
        public float jumpHeight = 3.0f;
        public float doubleJumpHeight = 4f;

        //Movement.
        [HideInInspector] public Vector3 currentVelocity;
        [HideInInspector] public bool isMoving = false;
        [HideInInspector] public bool canMove = true;
        public float movementAcceleration = 90.0f;
        public float walkSpeed = 4f;
        public float runSpeed = 6f;
        float rotationSpeed = 40f;
        public float groundFriction = 50f;
        [HideInInspector] public bool isRolling = false;
        public float rollSpeed = 8;
        public float rollduration = 0.35f;
        private int rollNumber;

        public float inAirSpeed = 6f;

        void Awake()
        {
            superCharacterController = GetComponent<SuperCharacterController>();
            playerController = GetComponent<DeathMatchPlayerController>();
            deathMatchPlayerInputController = GetComponent<DeathMatchPlayerInputController>();
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
            currentState = RPGCharacterStateFREE.Idle;
            rpgCharacterState = RPGCharacterStateFREE.Idle;
        }
        protected override void EarlyGlobalSuperUpdate()
        {
        }
        protected override void LateGlobalSuperUpdate()
        {
            transform.position += currentVelocity * superCharacterController.deltaTime;
            if (navMeshAgent != null)
            {
                if (useMeshNav)
                {
                    if (navMeshAgent.velocity.sqrMagnitude > 0)
                    {
                        animator.SetBool("Moving", true);
                        animator.SetFloat("Velocity Z", navMeshAgent.velocity.magnitude);
                    }
                    else
                    {
                        animator.SetFloat("Velocity Z", 0);
                    }
                }
            }
            if (!useMeshNav && !playerController.isDead && canMove)
            {
                if (currentVelocity.magnitude > 0 && deathMatchPlayerInputController.HasMoveInput())
                {
                    isMoving = true;
                    animator.SetBool("Moving", true);
                    animator.SetFloat("Velocity Z", currentVelocity.magnitude);
                }
                else
                {
                    isMoving = false;
                    animator.SetBool("Moving", false);
                }
            }
            if (!playerController.isStrafing)
            {
                if (deathMatchPlayerInputController.HasMoveInput() && canMove)
                {
                    RotateTowardsMovementDir();
                }
            }
            else
            {
                if (playerController.target != null)
                {
                    Strafing(playerController.target.transform.position);
                }
            }
        }

        private bool AcquiringGround()
        {
            return superCharacterController.currentGround.IsGrounded(false, 0.01f);
        }

        public bool MaintainingGround()
        {
            return superCharacterController.currentGround.IsGrounded(true, 0.5f);
        }

        public void RotateGravity(Vector3 up)
        {
            lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
        }
        private Vector3 LocalMovement()
        {
            return deathMatchPlayerInputController.moveInput;
        }

        private float CalculateJumpSpeed(float jumpHeight, float gravity)
        {
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }
        void Idle_EnterState()
        {
            superCharacterController.EnableSlopeLimit();
            superCharacterController.EnableClamping();
            canJump = true;
            doublejumped = false;
            canDoubleJump = false;
            animator.SetInteger("Jumping", 0);
        }
        void Idle_SuperUpdate()
        {
            if (deathMatchPlayerInputController.allowedInput && deathMatchPlayerInputController.inputJump)
            {
                currentState = RPGCharacterStateFREE.Jump;
                rpgCharacterState = RPGCharacterStateFREE.Jump;
                return;
            }
            if (!MaintainingGround())
            {
                currentState = RPGCharacterStateFREE.Fall;
                rpgCharacterState = RPGCharacterStateFREE.Fall;
                return;
            }
            if (deathMatchPlayerInputController.HasMoveInput() && canMove)
            {
                currentState = RPGCharacterStateFREE.Move;
                rpgCharacterState = RPGCharacterStateFREE.Move;
                return;
            }
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, groundFriction * superCharacterController.deltaTime);
        }

        void Idle_ExitState()
        {
        }

        void Move_SuperUpdate()
        {
            if (deathMatchPlayerInputController.allowedInput && deathMatchPlayerInputController.inputJump)
            {
                currentState = RPGCharacterStateFREE.Jump;
                rpgCharacterState = RPGCharacterStateFREE.Jump;
                return;
            }
            if (!MaintainingGround())
            {
                currentState = RPGCharacterStateFREE.Fall;
                rpgCharacterState = RPGCharacterStateFREE.Fall;
                return;
            }
            if (deathMatchPlayerInputController.HasMoveInput() && canMove && !playerController.warrior.isDead)
            {
                animator.SetFloat("Velocity X", 0F);
                if (playerController.isStrafing)
                {
                    currentVelocity = Vector3.MoveTowards(currentVelocity, LocalMovement() * walkSpeed, movementAcceleration * superCharacterController.deltaTime);
                    if (playerController.weapon != Weapon.RELAX)
                    {
                        if (playerController.target != null)
                        {
                            Strafing(playerController.target.transform.position);

                        }
                    }
                    return;
                }
                currentVelocity = Vector3.MoveTowards(currentVelocity, LocalMovement() * runSpeed, movementAcceleration * superCharacterController.deltaTime);
            }
            else
            {
                currentState = RPGCharacterStateFREE.Idle;
                rpgCharacterState = RPGCharacterStateFREE.Idle;
                return;
            }
        }

        void Jump_EnterState()
        {
            superCharacterController.DisableClamping();
            superCharacterController.DisableSlopeLimit();
            currentVelocity += superCharacterController.up * CalculateJumpSpeed(jumpHeight, gravity);
            if (playerController.weapon == Weapon.RELAX)
            {
                playerController.weapon = Weapon.UNARMED;
                animator.SetInteger("Weapon", 0);
            }
            canJump = false;
            animator.SetInteger("Jumping", 1);
            animator.SetTrigger("JumpTrigger");
        }

        void Jump_SuperUpdate()
        {
            Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(superCharacterController.up, currentVelocity);
            Vector3 verticalMoveDirection = currentVelocity - planarMoveDirection;
            if (Vector3.Angle(verticalMoveDirection, superCharacterController.up) > 90 && AcquiringGround())
            {
                currentVelocity = planarMoveDirection;
                currentState = RPGCharacterStateFREE.Idle;
                rpgCharacterState = RPGCharacterStateFREE.Idle;
                return;
            }
            planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, LocalMovement() * inAirSpeed, jumpAcceleration * superCharacterController.deltaTime);
            verticalMoveDirection -= superCharacterController.up * gravity * superCharacterController.deltaTime;
            currentVelocity = planarMoveDirection + verticalMoveDirection;
            //Can double jump if starting to fall.
            if (currentVelocity.y < 0)
            {
                DoubleJump();
            }
        }

        void DoubleJump_EnterState()
        {
            currentVelocity += superCharacterController.up * CalculateJumpSpeed(doubleJumpHeight, gravity);
            canDoubleJump = false;
            doublejumped = true;
            animator.SetInteger("Jumping", 3);
            animator.SetTrigger("JumpTrigger");
        }
        void DoubleJump_SuperUpdate()
        {
            Jump_SuperUpdate();
        }
        void DoubleJump()
        {
            if (!doublejumped)
            {
                canDoubleJump = true;
            }
            if (deathMatchPlayerInputController.inputJump && canDoubleJump && !doublejumped)
            {
                currentState = RPGCharacterStateFREE.DoubleJump;
                rpgCharacterState = RPGCharacterStateFREE.DoubleJump;
            }
        }
        void Fall_EnterState()
        {
            if (!doublejumped)
            {
                canDoubleJump = true;
            }
            superCharacterController.DisableClamping();
            superCharacterController.DisableSlopeLimit();
            canJump = false;
            animator.SetInteger("Jumping", 2);
            animator.SetTrigger("JumpTrigger");
        }
        void Fall_SuperUpdate()
        {
            if (AcquiringGround())
            {
                currentVelocity = Math3d.ProjectVectorOnPlane(superCharacterController.up, currentVelocity);
                currentState = RPGCharacterStateFREE.Idle;
                rpgCharacterState = RPGCharacterStateFREE.Idle;
                return;
            }
            DoubleJump();
            currentVelocity -= superCharacterController.up * gravity * superCharacterController.deltaTime;
        }

        void Roll_SuperUpdate()
        {
            if (rollNumber == 1)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, transform.forward * rollSpeed, groundFriction * superCharacterController.deltaTime);
            }
            if (rollNumber == 2)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, transform.right * rollSpeed, groundFriction * superCharacterController.deltaTime);
            }
            if (rollNumber == 3)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, -transform.forward * rollSpeed, groundFriction * superCharacterController.deltaTime);
            }
            if (rollNumber == 4)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, -transform.right * rollSpeed, groundFriction * superCharacterController.deltaTime);
            }
        }

        public void DirectionalRoll()
        {
            float angle = Vector3.Angle(deathMatchPlayerInputController.moveInput, -transform.forward);
            float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(deathMatchPlayerInputController.aimInput, transform.forward)));
            float signed_angle = angle * sign;
            float angle360 = (signed_angle + 180) % 360;
            if (angle360 > 315 || angle360 < 45)
            {
                StartCoroutine(_Roll(1));
            }
            if (angle360 > 45 && angle360 < 135)
            {
                StartCoroutine(_Roll(2));
            }
            if (angle360 > 135 && angle360 < 225)
            {
                StartCoroutine(_Roll(3));
            }
            if (angle360 > 225 && angle360 < 315)
            {
                StartCoroutine(_Roll(4));
            }
        }

        /// <summary>
        /// Character Roll.
        /// </summary>
        /// <param name="1">Forward.</param>
        /// <param name="2">Right.</param>
        /// <param name="3">Backward.</param>
        /// <param name="4">Left.</param>
        public IEnumerator _Roll(int roll)
        {
            rollNumber = roll;
            currentState = RPGCharacterStateFREE.Roll;
            rpgCharacterState = RPGCharacterStateFREE.Roll;
            if (playerController.weapon == Weapon.RELAX)
            {
                playerController.weapon = Weapon.UNARMED;
                animator.SetInteger("Weapon", 0);
            }
            animator.SetInteger("Action", rollNumber);
            animator.SetTrigger("RollTrigger");
            isRolling = true;
            playerController.canAction = false;
            yield return new WaitForSeconds(rollduration);
            isRolling = false;
            playerController.canAction = true;
            currentState = RPGCharacterStateFREE.Idle;
            rpgCharacterState = RPGCharacterStateFREE.Idle;
        }

        public void SwitchCollisionOff()
        {
            canMove = false;
            superCharacterController.enabled = false;
            animator.applyRootMotion = true;
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }

        public void SwitchCollisionOn()
        {
            canMove = true;
            superCharacterController.enabled = true;
            animator.applyRootMotion = false;
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }

        void RotateTowardsMovementDir()
        {
            if (playerController.warrior.isDead) return;
            if (deathMatchPlayerInputController.moveInput != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(deathMatchPlayerInputController.moveInput), Time.deltaTime * rotationSpeed);
            }
        }

        public void RotateTowardsTarget(Vector3 targetPosition, bool instant = false)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - new Vector3(transform.position.x, 0, transform.position.z));
            if (instant)
            {
                transform.eulerAngles = targetRotation.eulerAngles;
            }
            else
            {
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);

            }
        }

        void Aiming()
        {
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                if (Mathf.Abs(deathMatchPlayerInputController.inputAimHorizontal) > 0.8 || Mathf.Abs(deathMatchPlayerInputController.inputAimVertical) < -0.8)
                {
                    Vector3 joyDirection = new Vector3(deathMatchPlayerInputController.inputAimHorizontal, 0, -deathMatchPlayerInputController.inputAimVertical);
                    joyDirection = joyDirection.normalized;
                    Quaternion joyRotation = Quaternion.LookRotation(joyDirection);
                    transform.rotation = joyRotation;
                }
            }
            if (Input.GetJoystickNames().Length == 0)
            {
                Plane characterPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 mousePosition = new Vector3(0, 0, 0);
                float hitdist = 0.0f;
                if (characterPlane.Raycast(ray, out hitdist))
                {
                    mousePosition = ray.GetPoint(hitdist);
                }
                mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
                RotateTowardsTarget(mousePosition);
            }
            animator.SetFloat("Velocity X", transform.InverseTransformDirection(currentVelocity).x);
            animator.SetFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z);
        }
        void Strafing(Vector3 targetPosition)
        {
            animator.SetFloat("Velocity X", transform.InverseTransformDirection(currentVelocity).x);
            animator.SetFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z);
            RotateTowardsTarget(targetPosition);
        }

        public IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount)
        {
            isKnockback = true;
            StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
            yield return new WaitForSeconds(.1f);
            isKnockback = false;
        }

        IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount)
        {
            while (isKnockback)
            {
                rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
                yield return null;
            }
        }
        public void LockMovement()
        {
            canMove = false;
            animator.SetBool("Moving", false);
            animator.applyRootMotion = true;
            currentVelocity = new Vector3(0, 0, 0);
        }
        public void UnlockMovement()
        {
            canMove = true;
            animator.applyRootMotion = false;
        }
    }
}