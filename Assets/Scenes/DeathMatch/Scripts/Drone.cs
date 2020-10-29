using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Blue, Red, Random
}

public enum DroneState
{
    Wander, Chase, Attack
}
public class Drone : MonoBehaviour
{
    public Team Team => _team;
    [SerializeField]
    private Team _team;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    Material[] materials;

    [SerializeField]
    public Sprite uiSprite;
    public float _rayDistance = 10f;
    public float _stoppingDistance = 0.5f;

    private Vector3 _destination;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    public Warrior _target;
    public DroneState _currentState;

    private Creature creature;
    private Warrior warrior;
    private Rigidbody rb;
    private bool canAction = true;
    private bool canMove = true;
    private bool isKnockback;
    private float knockbackMultiplier = 1f;

    void Start()
    {
        creature = GetComponent<Creature>();
        warrior = GetComponent<Warrior>();
        rb = GetComponent<Rigidbody>();

        Light pointLight = GetComponentInChildren<Light>();
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        switch (_team)
        {
            case Team.Blue:
                pointLight.color = Color.blue;
                mesh.material = materials[0];
                break;
            case Team.Red:
                pointLight.color = Color.red;
                mesh.material = materials[1];
                break;
            case Team.Random:
                Team randomTeam = UnityEngine.Random.value > 0.5 ? Team.Blue : Team.Red;
                pointLight.color = randomTeam == Team.Red ? Color.red : Color.blue;
                mesh.material = randomTeam == Team.Red ? materials[1] : materials[0];
                break;
        }

        warrior.haveBeenAttacked += context => HaveBeenAttacked(context.damage, context.attacker);

    }

    // Update is called once per frame
    void Update()
    {

        switch (_currentState)
        {
            case DroneState.Wander:
                if (NeedDestination())
                {
                    GetDestination();
                }
                transform.rotation = _desiredRotation;
                transform.Translate(Vector3.forward * Time.deltaTime * creature.walkSpeed);

                var rayColor = IsPathBlocked() ? Color.red : Color.green;
                Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);

                while (IsPathBlocked())
                {
                    GetDestination();
                }

                var targetToAggro = warrior.CheckForAggro(CreatureType.Player);

                if (targetToAggro != null)
                {
                    _target = targetToAggro.GetComponent<Warrior>();
                    _currentState = DroneState.Chase;
                }

                if (_target != null && !_target.isDead)
                {
                    _currentState = DroneState.Chase;
                }

                break;
            case DroneState.Chase:
                if (_target == null || _target.isDead)
                {
                    _currentState = DroneState.Wander;
                    return;
                }
                if (Vector3.Distance(transform.position, _target.transform.position) < warrior.GetStats().attackRange)
                {
                    transform.LookAt(new Vector3(_target.transform.position.x, 0, _target.transform.position.z));
                    _currentState = DroneState.Attack;
                    return;
                }

                transform.LookAt(new Vector3(_target.transform.position.x, 0, _target.transform.position.z));
                transform.Translate(Vector3.forward * Time.deltaTime * creature.runSpeed);

                break;
            case DroneState.Attack:
                if (_target != null && !_target.isDead)
                {
                    if (!warrior.isAttacking)
                    {
                        StartCoroutine(AttackWarrior(_target, (damage) =>
                        {
                            // Debug.Log("Drone hit " + damage); 
                        }));
                    }
                    if (_target.warriorHealth.currentHealth > 0)
                    {
                        _currentState = DroneState.Chase;
                    }
                    else
                    {
                        _currentState = DroneState.Wander;
                    }
                }
                else
                {
                    _currentState = DroneState.Wander;
                }

                break;
        }
        if (_target != null && _target.isDead)
        {
            _target = null;
        }
    }

    private IEnumerator AttackWarrior(Warrior enemyWarrior, Warrior.WarriorDidAttackEnemy didAttack)
    {

        yield return null;
    }

    public bool IsSelected()
    {
        return transform.Find("TargetMarker").gameObject.activeInHierarchy;
    }

    public void SetSelected(bool flag)
    {
        transform.Find("TargetMarker").gameObject.SetActive(flag);
    }
    private bool IsPathBlocked()
    {
        Ray ray = new Ray(transform.position, _direction);
        RaycastHit[] hitSomething = Physics.RaycastAll(ray, _rayDistance, _layerMask);
        return hitSomething.Length > 0;
    }

    private void GetDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f) + new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0f, UnityEngine.Random.Range(-4.5f, 4.5f)));
        _destination = new Vector3(testPosition.x, 1f, testPosition.z);
        _direction = Vector3.Normalize(_destination - transform.position);
        _direction = new Vector3(_direction.x, 0f, _direction.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
        Debug.DrawRay(transform.position, _destination * _rayDistance, Color.green);
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

    private void HaveBeenAttacked(float damage, Warrior attacker)
    {
        GetHit(damage, attacker);
        if (_target == null)
        {
            _target = attacker.gameObject.GetComponent<Warrior>();
            _currentState = DroneState.Chase;
        }
    }

    public void GetHit(float damage, Warrior attacker)
    {
        Lock(true, true, true, 0.1f, 0.4f);
        // if (isBlocking)
        // {
        //     StartCoroutine(movementController._Knockback(-transform.forward, 3, 3));
        //     return;
        // }
        float force = damage / 100f;
        StartCoroutine(_Knockback(-transform.forward, force, force * 2));

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
            canMove = false;
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
            canMove = true;
        }
        if (actions)
        {
            canAction = true;
        }
    }

    public IEnumerator _Knockback(Vector3 knockDirection, float knockBackAmount, float variableAmount)
    {
        isKnockback = true;
        StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
        yield return new WaitForSeconds(.1f);
        isKnockback = false;
    }

    IEnumerator _KnockbackForce(Vector3 knockDirection, float knockBackAmount, float variableAmount)
    {
        while (isKnockback)
        {
            rb.AddForce(knockDirection * ((knockBackAmount + UnityEngine.Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
            yield return null;
        }
    }
}
