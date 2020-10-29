using UnityEngine;
using System.Collections;

namespace DeathMatchCharacterAnims
{
    public class DeathMatchBotMovementController : SuperStateMachine
    {
        private DeathMatchBotController botController;
        public UnityEngine.AI.NavMeshAgent navMeshAgent;
        private Rigidbody rb;
        private Animator animator;
        public RPGCharacterStateFREE rpgCharacterState;
        public bool useMeshNav = false;
        [HideInInspector] public Vector3 lookDirection { get; private set; }
        [HideInInspector] public bool isKnockback;
        public float knockbackMultiplier = 1f;
        [HideInInspector] public bool canJump;
        [HideInInspector] public bool canDoubleJump = false;
        bool doublejumped = false;
        public float gravity = 25.0f;
        public float jumpAcceleration = 5.0f;
        public float jumpHeight = 3.0f;
        public float doubleJumpHeight = 4f;
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
            botController = GetComponent<DeathMatchBotController>();
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
            transform.position += currentVelocity * Time.deltaTime;
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
            if (!useMeshNav && !botController.isDead && canMove)
            {
                if (currentVelocity.magnitude > 0 && botController.HasMoveInput())
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
            if (!botController.isStrafing)
            {
                if (botController.HasMoveInput() && canMove)
                {
                    RotateTowardsMovementDir();
                }
            }
            else
            {
                if (botController.target != null)
                {
                    Strafing(botController.target.transform.position);
                }
            }
        }

        private bool AcquiringGround()
        {
            return false;
        }

        public bool MaintainingGround()
        {
            return true;
        }

        public void RotateGravity(Vector3 up)
        {
            lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
        }

        /// <summary>
        /// Constructs a vector representing our movement local to our lookDirection, which is controlled by the camera.
        /// </summary>
        private Vector3 LocalMovement()
        {
            return botController.moveInput;
        }
        private float CalculateJumpSpeed(float jumpHeight, float gravity)
        {
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }
        void Idle_EnterState()
        {
            canJump = true;
            doublejumped = false;
            canDoubleJump = false;
            animator.SetInteger("Jumping", 0);
        }

        void Idle_SuperUpdate()
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, groundFriction * Time.deltaTime);
        }

        void Idle_ExitState()
        {
        }

        void Move_SuperUpdate()
        {
            if (botController.HasMoveInput() && canMove)
            {
                animator.SetFloat("Velocity X", 0F);
                if (botController.isStrafing)
                {
                    currentVelocity = Vector3.MoveTowards(currentVelocity, LocalMovement() * walkSpeed, movementAcceleration * Time.deltaTime);
                    if (botController.weapon != Weapon.RELAX)
                    {
                        if (botController.target != null)
                        {
                            Strafing(botController.target.transform.position);

                        }
                    }
                    return;
                }
                //Run.
                currentVelocity = Vector3.MoveTowards(currentVelocity, LocalMovement() * runSpeed, movementAcceleration * Time.deltaTime);
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
            if (botController.weapon == Weapon.RELAX)
            {
                botController.weapon = Weapon.UNARMED;
                animator.SetInteger("Weapon", 0);
            }
            canJump = false;
            animator.SetInteger("Jumping", 1);
            animator.SetTrigger("JumpTrigger");
        }

        void Jump_SuperUpdate()
        {
        }

        void DoubleJump_EnterState()
        {
        }


        void Fall_EnterState()
        {
            if (!doublejumped)
            {
                canDoubleJump = true;
            }
            canJump = false;
            animator.SetInteger("Jumping", 2);
            animator.SetTrigger("JumpTrigger");
        }

        void Fall_SuperUpdate()
        {
        }

        void Roll_SuperUpdate()
        {
            if (rollNumber == 1)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, transform.forward * rollSpeed, groundFriction * Time.deltaTime);
            }
            if (rollNumber == 2)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, transform.right * rollSpeed, groundFriction * Time.deltaTime);
            }
            if (rollNumber == 3)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, -transform.forward * rollSpeed, groundFriction * Time.deltaTime);
            }
            if (rollNumber == 4)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, -transform.right * rollSpeed, groundFriction * Time.deltaTime);
            }
        }

        public void DirectionalRoll()
        {
            //Check which way the dash is pressed relative to the character facing.
            float angle = Vector3.Angle(botController.moveInput, -transform.forward);
            Vector3 aimInput = Vector3.zero;
            float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(aimInput, transform.forward)));
            //Angle in [-179,180].
            float signed_angle = angle * sign;
            //Angle in 0-360.
            float angle360 = (signed_angle + 180) % 360;
            //Deternime the animation to play based on the angle.
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
            if (botController.weapon == Weapon.RELAX)
            {
                botController.weapon = Weapon.UNARMED;
                animator.SetInteger("Weapon", 0);
            }
            animator.SetInteger("Action", rollNumber);
            animator.SetTrigger("RollTrigger");
            isRolling = true;
            botController.canAction = false;
            yield return new WaitForSeconds(rollduration);
            isRolling = false;
            botController.canAction = true;
            currentState = RPGCharacterStateFREE.Idle;
            rpgCharacterState = RPGCharacterStateFREE.Idle;
        }

        public void SwitchCollisionOff()
        {
            canMove = false;
            animator.applyRootMotion = true;
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }

        public void SwitchCollisionOn()
        {
            canMove = true;
            animator.applyRootMotion = false;
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }

        void RotateTowardsMovementDir()
        {
            if (botController.moveInput != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(botController.moveInput), Time.deltaTime * rotationSpeed);
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