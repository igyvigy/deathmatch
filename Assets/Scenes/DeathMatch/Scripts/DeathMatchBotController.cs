using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeathMatchCharacterAnims;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(DeathMatchBotWeaponController))]
public class DeathMatchBotController : MonoBehaviour
{

    public enum BotState
    {
        Wander, Chase, Attack
    }
    public BotState _currentState;
    public AudioClip hit;
    public AudioClip swordPickupClip;
    public AudioClip healPickupClip;
    public AudioClip getHitClip;
    public AudioClip dieClip;
    AudioSource audioSource;
    [HideInInspector] public DeathMatchBotWeaponController weaponController;
    [SerializeField]
    private LayerMask _layerMask;
    private Rigidbody rb;
    [HideInInspector] public Warrior warrior;
    [HideInInspector] public Creature creature;
    [HideInInspector] public Animator animator;
    [HideInInspector] public IKHandsFREE ikHands;
    public Weapon weapon = Weapon.UNARMED;
    public Warrior target;
    private int currentTargetInstanceId;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isBlocking = false;
    [HideInInspector] public bool isStrafing = false;
    [HideInInspector] public bool canAction = true;
    [HideInInspector] public bool canMove = true;
    private bool? lastValueForNextTargetAction;
    private bool? lastValueForPrevTargetAction;
    public float animationSpeed = 1;
    private bool isAutoAttackingCurrentTarget = false;
    private Vector3 _destination;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    public float _rayDistance = 10f;
    public float _stoppingDistance = 0.5f;
    public Vector3 moveInput;
    private bool isKnockback = false;


    #region Initialization

    void Awake()
    {
        weaponController = GetComponent<DeathMatchBotWeaponController>();
        rb = GetComponent<Rigidbody>();

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

        if (target != null && target.isDead)
        {
            target = null;
        }

        UpdateAnimationSpeed();
        if (!canMove)
        {
            animator.SetBool("Moving", false);
            animator.SetFloat("Velocity Z", 0f);
            return;
        }
        switch (_currentState)
        {
            case BotState.Wander:
                if (NeedDestination())
                {
                    GetDestination();
                }
                transform.rotation = _desiredRotation;
                transform.Translate(Vector3.forward * Time.deltaTime * creature.walkSpeed);
                animator.SetBool("Moving", true);
                animator.SetFloat("Velocity Z", 3f);
                var rayColor = IsPathBlocked() ? Color.red : Color.green;
                Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);
                while (IsPathBlocked())
                {
                    GetDestination();
                }
                var targetToAggro = warrior.CheckForAggro(CreatureType.Player);
                if (targetToAggro != null)
                {
                    target = targetToAggro.GetComponent<Warrior>();
                    _currentState = BotState.Chase;
                }
                if (target != null && !target.isDead)
                {
                    _currentState = BotState.Chase;
                }
                break;
            case BotState.Chase:
                if (target == null || target.isDead)
                {
                    _currentState = BotState.Wander;
                    return;
                }

                if (Vector3.Distance(transform.position, target.transform.position) * 0.95 <= warrior.GetStats().attackRange)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.smoothDeltaTime * creature.turnSpeed);
                    animator.SetBool("Moving", false);
                    animator.SetFloat("Velocity Z", 0f);
                    _currentState = BotState.Attack;
                    return;
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.smoothDeltaTime * creature.turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * creature.runSpeed);
                animator.SetBool("Moving", true);
                animator.SetFloat("Velocity Z", 6f);
                break;
            case BotState.Attack:
                if (target != null && !target.isDead)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) * 0.95 <= warrior.GetStats().attackRange)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.smoothDeltaTime * creature.turnSpeed);
                        Attack(1);
                        if (target.isDead)
                        {
                            _currentState = BotState.Wander;
                        }
                    }
                    else
                    {
                        _currentState = BotState.Chase;
                    }
                }
                else
                {
                    _currentState = BotState.Wander;
                }
                break;
        }
    }

    public bool HasMoveInput()
    {

        if (moveInput != Vector3.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateAnimationSpeed()
    {
        animator.SetFloat("AnimationSpeed", animationSpeed);
    }

    #endregion

    #region Aiming / Turning

    //Turning.
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

    //0 = No side
    //1 = Left
    //2 = Right
    //weaponNumber 0 = Unarmed
    //weaponNumber 1 = 2H Sword

    public void Attack(int attackSide)
    {
        if (warrior.isDead) return;
        if (target != null && !target.isDead)
        {
            isAutoAttackingCurrentTarget = true;
            if (Vector3.Distance(transform.position, target.transform.position) > warrior.GetStats().attackRange * DeathMatchWeapon.i.AttackRange(weapon))
            {
                return;
            }
            else
            {
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

            if (weapon == Weapon.RELAX)
            {
                weapon = Weapon.UNARMED;
                animator.SetInteger("Weapon", 0);
            }
            //Armed or Unarmed.
            if (weapon == Weapon.UNARMED || weapon == Weapon.ARMED || weapon == Weapon.ARMEDSHIELD)
            {
                int maxAttacks = 3;
                //Left attacks.
                if (attackSide == 1)
                {
                    animator.SetInteger("AttackSide", 1);
                    attackNumber = Random.Range(1, maxAttacks + 1);
                }
                //Right attacks.
                else if (attackSide == 2)
                {
                    animator.SetInteger("AttackSide", 2);
                    attackNumber = Random.Range(4, maxAttacks + 4);
                }
                //Set the Locks.
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
            // }
            // }
            //Trigger the animation.
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

    }

    void Strafing()
    {

    }

    void Rolling()
    {

    }

    private void HaveBeenAttacked(float damage, Vector3 attackDirection, Warrior attacker)
    {
        if (damage > 50)
        {
            GetHit(attackDirection, attacker.GetStats().attack);
            if (target == null)
            {
                target = attacker.gameObject.GetComponent<Warrior>();
            }
        }
        if (warrior.isDead)
        {
            if (transform.Find("Selection") != null)
            {
                transform.Find("Selection").gameObject.SetActive(false);
            }
            Death();
        }
    }

    public void GetHit(Vector3 attackDirection, float knockbackMultiplier)
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
                StartCoroutine(_Knockback(transform.forward, 3, 3, knockbackMultiplier));
                return;
            }
            //Apply directional knockback force.
            if (hitNumber <= 1)
            {
                StartCoroutine(_Knockback(transform.forward, 8, 4, knockbackMultiplier));
            }
            else if (hitNumber == 2)
            {
                StartCoroutine(_Knockback(-transform.forward, 8, 4, knockbackMultiplier));
            }
            else if (hitNumber == 3)
            {
                StartCoroutine(_Knockback(-transform.forward, 8, 4, knockbackMultiplier));
            }
            else if (hitNumber == 4)
            {
                StartCoroutine(_Knockback(-transform.right, 8, 4, knockbackMultiplier));
            }
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

    void LockMovement()
    {
        canMove = false;
    }

    /// <summary>
    /// Let character move and act again.
    /// </summary>
    void UnLock(bool movement, bool actions)
    {
        if (movement)
        {
            canMove = true;
        }
        if (actions)
        {
            canAction = true;
        }
    }

    public IEnumerator _Knockback(Vector3 knockDirection, float knockBackAmount, float variableAmount, float knockbackMultiplier)
    {
        isKnockback = true;
        StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount, knockbackMultiplier));
        yield return new WaitForSeconds(.1f);
        isKnockback = false;
    }

    IEnumerator _KnockbackForce(Vector3 knockDirection, float knockBackAmount, float variableAmount, float knockbackMultiplier)
    {
        while (isKnockback)
        {
            rb.AddForce(knockDirection * ((knockBackAmount + UnityEngine.Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
            yield return null;
        }
    }

    #endregion

    #region Misc

    private bool IsPathBlocked()
    {
        Vector3 from = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Ray ray = new Ray(from, _direction);
        RaycastHit[] hitSomething = Physics.RaycastAll(ray, _rayDistance, _layerMask);
        Debug.DrawRay(from, _direction * _rayDistance, Color.green);

        return hitSomething.Length > 0;
    }

    private void GetDestination()
    {
        DeathMatch dm = TagResolver.i.deathMatch;
        if (dm.GetAliveEnemiesCount() < 7)
        {
            Vector3 testPosition = dm.players[0].transform.position;
            _destination = new Vector3(testPosition.x, 1f, testPosition.z);
            _direction = Vector3.Normalize(_destination - transform.position);
            _direction = new Vector3(_direction.x, 0f, _direction.z);
            _desiredRotation = Quaternion.LookRotation(_direction);
        }
        else
        {
            Vector3 testPosition = (transform.position + (transform.forward * 4f) + new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0f, UnityEngine.Random.Range(-4.5f, 4.5f)));
            _destination = new Vector3(testPosition.x, 1f, testPosition.z);
            _direction = Vector3.Normalize(_destination - transform.position);
            _direction = new Vector3(_direction.x, 0f, _direction.z);
            _desiredRotation = Quaternion.LookRotation(_direction);
        }

    }

    private bool NeedDestination()
    {
        if (_destination == Vector3.zero)
        {
            return true;
        }
        var distance = Vector3.Distance(transform.position, _destination);
        if (distance <= _stoppingDistance)
        {
            return true;
        }
        return false;
    }

    public void Hit()
    {
        if (target == null) return;
        Warrior enemyWarrior = target.GetComponent<Warrior>();
        audioSource.PlayOneShot(hit, 0.7F);
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
        // audioSource.PlayOneShot(hit, 0.7F);
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
            LockMovement();
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
