using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DeathMatchCharacterAnims;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(DeathMatchPlayerInputController))]
[RequireComponent(typeof(DeathMatchPlayerMovementController))]
[RequireComponent(typeof(DeathMatchPlayerWeaponController))]
public class DeathMatchPlayerController : MonoBehaviour
{
    public AudioClip hitClip;
    public AudioClip swordPickupClip;
    public AudioClip healPickupClip;
    public AudioClip getHitClip;
    public AudioClip dieClip;
    AudioSource audioSource;

    [SerializeField] public Sprite selectionSprite;
    //Components.
    [HideInInspector] public DeathMatchPlayerMovementController movementController;
    [HideInInspector] public DeathMatchPlayerWeaponController weaponController;
    [HideInInspector] public DeathMatchPlayerInputController inputController;
    [HideInInspector] public Warrior warrior;
    [HideInInspector] public Creature creature;
    [HideInInspector] public Animator animator;
    [HideInInspector] public IKHandsFREE ikHands;
    public Weapon weapon = Weapon.UNARMED;
    public GameObject target;
    private int currentTargetInstanceId;

    //Strafing/action.
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isBlocking = false;
    [HideInInspector] public bool isStrafing = false;
    [HideInInspector] public bool canAction = true;

    private bool? lastValueForNextTargetAction;
    private bool? lastValueForPrevTargetAction;

    public float animationSpeed = 1;

    private bool isAutoAttackingCurrentTarget = false;

    #region Initialization

    void Awake()
    {
        movementController = GetComponent<DeathMatchPlayerMovementController>();
        weaponController = GetComponent<DeathMatchPlayerWeaponController>();
        inputController = GetComponent<DeathMatchPlayerInputController>();
        warrior = GetComponent<Warrior>();
        creature = GetComponent<Creature>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (animator == null)
        {
            Debug.LogError("ERROR: There is no animator for character.");
            Destroy(this);
        }
        ikHands = GetComponent<IKHandsFREE>();
        weapon = Weapon.UNARMED;
        animator.SetInteger("Weapon", 0);
        animator.SetInteger("WeaponSwitch", -1);

    }

    void Start()
    {
        movementController.SwitchCollisionOn();
        warrior.haveBeenAttacked += context => HaveBeenAttacked(context.damage, context.attackDirection, context.attacker);
    }

    #endregion

    #region Updates

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case Constants.k_TwoHandSwordSpawnTag:
                other.gameObject.SetActive(false);
                weaponController.SwitchWeaponTwoHand(1);
                audioSource.PlayOneShot(swordPickupClip, 0.7F);
                break;
            case Constants.k_HealSpawnTag:
                other.gameObject.SetActive(false);
                warrior.warriorHealth.SetCurrentHealth(warrior.GetStats().maxHealth);
                audioSource.PlayOneShot(healPickupClip, 0.7F);
                break;

        }
    }

    void Update()
    {
        if (warrior.isDead) return;
        if (target != null && target.GetComponent<Warrior>().isDead)
        {
            target = null;
        }

        UpdateAnimationSpeed();
        if (inputController.HasMoveInput())
        {
            if (isAutoAttackingCurrentTarget)
            {
                isAutoAttackingCurrentTarget = false;
                movementController.useMeshNav = false;
                movementController.navMeshAgent.enabled = false;
            }
        }
        if (movementController.MaintainingGround())
        {
            if (inputController.inputDeath)
            {
                if (isDead)
                {
                    Revive();
                }
            }
            if (canAction)
            {
                if (!isBlocking)
                {
                    Strafing();
                    Rolling();
                    if (inputController.inputDeath)
                    {
                        if (!isDead)
                        {
                            Death();
                        }
                        else
                        {
                            Revive();
                        }
                    }
                    if (inputController.inputAttackL)
                    {
                        Attack(1);
                    }
                    if (inputController.inputAttackR)
                    {
                        Attack(2);
                    }
                    if (inputController.inputSwitchUpDown)
                    {
                        if (weaponController.isSwitchingFinished)
                        {
                            if (weapon == Weapon.UNARMED)
                            {
                                weaponController.SwitchWeaponTwoHand(1);
                            }
                            else
                            {
                                weaponController.SwitchWeaponTwoHand(0);
                            }
                        }
                    }
                    if (lastValueForNextTargetAction == null) lastValueForNextTargetAction = inputController.inputNextTarget;
                    if (lastValueForNextTargetAction != inputController.inputNextTarget)
                    {
                        if (lastValueForNextTargetAction == true)
                        {
                            SelectTargetFromWarriorsNearby(warrior.aggroConeRadius, true);
                        }
                    }
                    lastValueForNextTargetAction = inputController.inputNextTarget;

                    if (lastValueForPrevTargetAction == null) lastValueForPrevTargetAction = inputController.inputPrevTarget;
                    if (lastValueForPrevTargetAction != inputController.inputPrevTarget)
                    {
                        if (lastValueForPrevTargetAction == true)
                        {
                            SelectTargetFromWarriorsNearby(warrior.aggroConeRadius, false);
                        }
                    }
                    lastValueForPrevTargetAction = inputController.inputPrevTarget;
                    if (isAutoAttackingCurrentTarget)
                    {
                        if (target == null)
                        {
                            isAutoAttackingCurrentTarget = false;
                            movementController.useMeshNav = false;
                            movementController.navMeshAgent.enabled = false;
                            return;
                        }

                        if (Vector3.Distance(transform.position, target.transform.position) <= warrior.GetStats().attackRange * DeathMatchWeapon.i.AttackRange(weapon))
                        {
                            movementController.useMeshNav = false;
                            movementController.navMeshAgent.enabled = false;
                            Attack(UnityEngine.Random.Range(1, 3));
                        }
                        else
                        {
                            movementController.useMeshNav = true;
                            movementController.navMeshAgent.enabled = true;
                            Vector3 destination = new Vector3(target.transform.position.x, 0, target.transform.position.z);
                            movementController.navMeshAgent.destination = destination;
                        }

                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Time.timeScale != 1)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0.005f;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale != 1)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
        if (inputController.inputStrafe)
        {
            animator.SetBool("Strafing", true);
        }
        else
        {
            animator.SetBool("Strafing", false);
        }
    }

    void UpdateAnimationSpeed()
    {
        animator.SetFloat("AnimationSpeed", animationSpeed);
    }

    #endregion

    #region Aiming / Turning

    public IEnumerator _Turning(int direction)
    {
        if (direction == 1)
        {
            Lock(true, true, true, 0, 0.55f);
            animator.SetTrigger("TurnLeftTrigger");
        }
        if (direction == 2)
        {
            Lock(true, true, true, 0, 0.55f);
            animator.SetTrigger("TurnRightTrigger");
        }
        yield return null;
    }

    #endregion

    #region Combat

    public void Attack(int attackSide)
    {
        if (warrior.isDead) return;
        if (target != null && !target.GetComponent<Warrior>().isDead)
        {
            isAutoAttackingCurrentTarget = true;
            if (Vector3.Distance(transform.position, target.transform.position) > warrior.GetStats().attackRange * DeathMatchWeapon.i.AttackRange(weapon))
            {
                return;
            }
            else
            {
                movementController.RotateTowardsTarget(new Vector3(target.transform.position.x, 0, target.transform.position.z), true);
            }
        }
        else
        {
            isAutoAttackingCurrentTarget = false;
            return;
        }

        int attackNumber = 0;
        if (canAction)
        {
            if (movementController.MaintainingGround())
            {
                if (!movementController.isMoving)
                {
                    if (weapon == Weapon.RELAX)
                    {
                        weapon = Weapon.UNARMED;
                        animator.SetInteger("Weapon", 0);
                    }
                    if (weapon == Weapon.UNARMED || weapon == Weapon.ARMED || weapon == Weapon.ARMEDSHIELD)
                    {
                        int maxAttacks = 3;
                        if (attackSide == 1)
                        {
                            animator.SetInteger("AttackSide", 1);
                            attackNumber = Random.Range(1, maxAttacks + 1);
                        }
                        else if (attackSide == 2)
                        {
                            animator.SetInteger("AttackSide", 2);
                            attackNumber = Random.Range(4, maxAttacks + 4);
                        }
                        if (attackSide != 3)
                        {
                            Lock(true, true, true, 0, 1.25f);
                        }
                    }
                    else
                    {
                        int maxAttacks = 6;
                        attackNumber = Random.Range(1, maxAttacks);
                        if (weapon == Weapon.TWOHANDSWORD)
                        {
                            Lock(true, true, true, 0, 0.85f);
                        }
                        else
                        {
                            Lock(true, true, true, 0, 0.75f);
                        }
                    }
                }
            }
            animator.SetInteger("Action", attackNumber);
            if (attackSide == 3)
            {
                animator.SetTrigger("AttackDualTrigger");
            }
            else
            {
                animator.SetTrigger("AttackTrigger");
            }
        }
    }

    public void AttackKick(int kickSide)
    {
        if (movementController.MaintainingGround())
        {
            if (weapon == Weapon.RELAX)
            {
                weapon = Weapon.UNARMED;
                animator.SetInteger("Weapon", 0);
            }
            animator.SetInteger("Action", kickSide);
            animator.SetTrigger("AttackKickTrigger");
            Lock(true, true, true, 0, 0.9f);
        }
    }

    void Strafing()
    {
        if (inputController.inputStrafe && weapon != Weapon.RIFLE)
        {
            if (weapon != Weapon.RELAX)
            {
                animator.SetBool("Strafing", true);
                isStrafing = true;
            }
        }
        else
        {
            isStrafing = false;
            animator.SetBool("Strafing", false);
        }
    }

    void Rolling()
    {
        if (!movementController.isRolling)
        {
            if (inputController.inputRoll)
            {
                movementController.DirectionalRoll();
            }
        }
    }

    private void HaveBeenAttacked(float damage, Vector3 attackDirection, Warrior attacker)
    {
        if (damage > 50)
        {
            GetHit(attackDirection);
        }
        if (target == null)
        {
            target = attacker.gameObject;
            SetTargetSelected(target, true);
            currentTargetInstanceId = target.GetInstanceID();
        }
        if (warrior.isDead)
        {
            Death();
        }
    }

    private void SetTargetSelected(GameObject target, bool flag)
    {
        if (target.transform.Find("Selection") != null)
        {
            target.transform.Find("Selection").gameObject.SetActive(flag);
        }
        else
        {
            GameObject selection = new GameObject();
            selection.name = "Selection";
            SpriteRenderer renderer = selection.AddComponent<SpriteRenderer>();
            renderer.sprite = selectionSprite;
            selection.transform.SetParent(target.transform);
            selection.transform.localPosition = Vector3.zero;
            selection.transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
            selection.transform.localScale = new Vector3(2f, 2f, 2f);
            selection.SetActive(flag);
        }
    }

    public void GetHit(Vector3 attackDirection)
    {
        audioSource.PlayOneShot(getHitClip, 0.5F);
        if (weapon == Weapon.RELAX)
        {
            weapon = Weapon.UNARMED;
            animator.SetInteger("Weapon", 0);
        }
        if (weapon != Weapon.RIFLE || weapon != Weapon.TWOHANDCROSSBOW)
        {
            int hits = 5;
            if (isBlocking)
            {
                hits = 2;
            }
            int hitNumber = Random.Range(1, hits + 1);
            animator.SetInteger("Action", hitNumber);
            animator.SetTrigger("GetHitTrigger");
            Lock(true, true, true, 0.1f, 0.4f);
            if (isBlocking)
            {
                StartCoroutine(movementController._Knockback(-transform.forward, 3, 3));
                return;
            }
            //Apply directional knockback force.
            if (hitNumber <= 1)
            {
                StartCoroutine(movementController._Knockback(-transform.forward, 8, 4));
            }
            else if (hitNumber == 2)
            {
                StartCoroutine(movementController._Knockback(transform.forward, 8, 4));
            }
            else if (hitNumber == 3)
            {
                StartCoroutine(movementController._Knockback(transform.forward, 8, 4));
            }
            else if (hitNumber == 4)
            {
                StartCoroutine(movementController._Knockback(-transform.right, 8, 4));
            }
        }
    }

    private void SelectTargetFromWarriorsNearby(float range, bool isGoingForward)
    {
        List<GameObject> results = new List<GameObject>();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        int i = 0;
        while (i < hitColliders.Length)
        {
            GameObject o = hitColliders[i].gameObject;

            Warrior warrior = o.GetComponent<Warrior>();
            if (warrior != null && !warrior.isDead)
            {
                if (o != gameObject) // skip self
                {
                    results.Add(o);
                }
            }
            i++;
        }

        if (results.Count > 0)
        {
            results.Sort(delegate (GameObject a, GameObject b) // sort from left to right
            {
                return (a.transform.position.x).CompareTo(b.transform.position.x);
            });
            int nextTargetIndex = 0;
            if (target != null) // there is a target. Try shift to next
            {
                int currentTargetIndex = results.IndexOf(target);

                if (isGoingForward)
                {
                    nextTargetIndex = currentTargetIndex + 1;
                    if (nextTargetIndex == results.Count)
                    {
                        nextTargetIndex = 0;
                    }
                }
                else
                {
                    nextTargetIndex = currentTargetIndex - 1;
                    if (nextTargetIndex < 0)
                    {
                        nextTargetIndex = results.Count - 1;
                    }
                }
            }
            else
            {
                // keep index 0;
            }
            GameObject prevTarget = target;
            if (prevTarget)
            {
                SetTargetSelected(prevTarget, false);
            }
            target = results[nextTargetIndex].gameObject;
            SetTargetSelected(target, true);
            currentTargetInstanceId = target.GetInstanceID();
        }

    }

    public void Death()
    {
        animator.SetTrigger("Death1Trigger");
        Lock(true, true, false, 0.1f, 0f);
        isDead = true;
        audioSource.PlayOneShot(dieClip, 0.7F);
    }

    public void Revive()
    {
        animator.SetTrigger("Revive1Trigger");
        Lock(true, true, true, 0f, 1f);
        isDead = false;
    }

    #endregion

    #region Actions

    /// <summary>
    /// Keep character from doing actions.
    /// </summary>
    void LockAction()
    {
        canAction = false;
    }

    /// <summary>
    /// Let character move and act again.
    /// </summary>
    void UnLock(bool movement, bool actions)
    {
        if (movement)
        {
            movementController.UnlockMovement();
        }
        if (actions)
        {
            canAction = true;
        }
    }

    #endregion

    #region Misc

    //Placeholder functions for Animation events.
    public void Hit()
    {
        if (target == null) return;
        Warrior enemyWarrior = target.GetComponent<Warrior>();
        audioSource.PlayOneShot(hitClip, 0.7F);
        warrior.GiveDamage(enemyWarrior, weapon);
    }

    public void Shoot()
    {
    }

    public void FootR()
    {
    }

    public void FootL()
    {
    }

    public void Land()
    {
    }

    IEnumerator _GetCurrentAnimationLength()
    {
        yield return new WaitForEndOfFrame();
        float f = (float)animator.GetCurrentAnimatorClipInfo(0).Length;
        Debug.Log(f);
    }

    /// <summary>
    /// Lock character movement and/or action, on a delay for a set time.
    /// </summary>
    /// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
    /// <param name="lockAction">If set to <c>true</c> lock action.</param>
    /// <param name="timed">If set to <c>true</c> timed.</param>
    /// <param name="delayTime">Delay time.</param>
    /// <param name="lockTime">Lock time.</param>
    public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
    {
        StopCoroutine("_Lock");
        StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
    }

    //Timed -1 = infinite, 0 = no, 1 = yes.
    public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
    {
        if (delayTime > 0)
        {
            yield return new WaitForSeconds(delayTime);
        }
        if (lockMovement)
        {
            movementController.LockMovement();
        }
        if (lockAction)
        {
            LockAction();
        }
        if (timed)
        {
            if (lockTime > 0)
            {
                yield return new WaitForSeconds(lockTime);
            }
            UnLock(lockMovement, lockAction);
        }
    }

    /// <summary>
    /// Sets the animator state.
    /// </summary>
    /// <param name="weapon">Weapon.</param>
    /// <param name="weaponSwitch">Weapon switch.</param>
    /// <param name="Lweapon">Lweapon.</param>
    /// <param name="Rweapon">Rweapon.</param>
    /// <param name="weaponSide">Weapon side.</param>
    void SetAnimator(int weapon, int weaponSwitch, int Lweapon, int Rweapon, int weaponSide)
    {
        Debug.Log("SETANIMATOR: Weapon:" + weapon + " Weaponswitch:" + weaponSwitch + " Lweapon:" + Lweapon + " Rweapon:" + Rweapon + " Weaponside:" + weaponSide);
        //Set Weapon if applicable.
        if (weapon != -2)
        {
            animator.SetInteger("Weapon", weapon);
        }
        //Set WeaponSwitch if applicable.
        if (weaponSwitch != -2)
        {
            animator.SetInteger("WeaponSwitch", weaponSwitch);
        }
        //Set left weapon if applicable.
        if (Lweapon != -1)
        {
            weaponController.leftWeapon = Lweapon;
            animator.SetInteger("LeftWeapon", Lweapon);
            //Set Shield.
            if (Lweapon == 7)
            {
                animator.SetBool("Shield", true);
            }
            else
            {
                animator.SetBool("Shield", false);
            }
        }
        //Set weapon side if applicable.
        if (weaponSide != -1)
        {
            animator.SetInteger("LeftRight", weaponSide);
        }
        SetWeaponState(weapon);
    }

    public void SetWeaponState(int weaponNumber)
    {
        if (weaponNumber == -1)
        {
            weapon = Weapon.RELAX;
        }
        else if (weaponNumber == 0)
        {
            weapon = Weapon.UNARMED;
        }
        else if (weaponNumber == 1)
        {
            weapon = Weapon.TWOHANDSWORD;
        }
    }

    public void AnimatorDebug()
    {
        Debug.Log("ANIMATOR SETTINGS---------------------------");
        Debug.Log("Moving: " + animator.GetBool("Moving"));
        Debug.Log("Strafing: " + animator.GetBool("Strafing"));
        Debug.Log("Stunned: " + animator.GetBool("Stunned"));
        Debug.Log("Weapon: " + animator.GetInteger("Weapon"));
        Debug.Log("WeaponSwitch: " + animator.GetInteger("WeaponSwitch"));
        Debug.Log("Jumping: " + animator.GetInteger("Jumping"));
        Debug.Log("Action: " + animator.GetInteger("Action"));
        Debug.Log("Velocity X: " + animator.GetFloat("Velocity X"));
        Debug.Log("Velocity Z: " + animator.GetFloat("Velocity Z"));
    }

    #endregion

}
