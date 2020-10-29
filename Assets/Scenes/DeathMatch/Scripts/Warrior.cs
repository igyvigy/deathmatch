using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DeathMatchCharacterAnims;
public class Warrior : MonoBehaviour
{

    [Serializable]
    public class Stats
    {
        public float attack = 10f;
        public float defense = 1.0f;
        public float attackSpeed = 0.8f;
        public float baseDefaultAttackDuration = 2.767f;
        public float baseDefaultAttackHit = 1.0f;
        public float attackRange = 2.0f;
        public float maxHealth = 500f;
        public float minAttackToGetHit = 40f;

        public Stats(
            float attack,
            float defense,
            float attackSpeed,
            float attackRange,
            float maxHealth,
            float minAttackToGetHit)
        {
            this.attack = attack;
            this.defense = defense;
            this.attackSpeed = attackSpeed;
            this.attackRange = attackRange;
            this.maxHealth = maxHealth;
            this.minAttackToGetHit = minAttackToGetHit;
        }
    }

    [SerializeField] public Stats stats;
    public WarriorHealth warriorHealth;
    public bool isDead = false;
    public float aggroConeRadius = 7f;
    public bool isAttacking = false;
    public int level = 1;
    private float _defaultAttack = 10f;
    private float _defaultDefense = 2f;
    private float _defaultAttackSpeed = 0.8f;
    private float _defaultBaseDefaultAttackDuration = 2.767f;
    private float _defaultBaseDefaultAttackHit = 1.0f;
    private float _defaultAttackRange = 2.0f;
    private float _defaultMaxHealth = 500f;
    private float _defaultCurrentHealth = 400f;
    private float _defaultAggroConeRadius = 7f;

    public struct WarriorAttacked
    {
        public Warrior attacker;
        public float damage;
        public Vector3 attackDirection;
        public WarriorAttacked(float damage, Vector3 attackDirection, Warrior attacker)
        {
            this.damage = damage;
            this.attackDirection = attackDirection;
            this.attacker = attacker;
        }
    }

    public event Action<WarriorAttacked> haveBeenAttacked;

    void Start()
    {
        warriorHealth = GetComponentInParent<WarriorHealth>();
    }

    public Stats GetStats()
    {
        return new Stats(
            stats.attack * level * 1f,
            (stats.defense * level * 1f),
            stats.attackSpeed * level * 1f,
            stats.attackRange,
            stats.maxHealth * level * 1f,
            stats.minAttackToGetHit * level
            );
    }

    Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    public Transform CheckForAggro(CreatureType victimType)
    {
        float aggroRadius = aggroConeRadius;
        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = new Vector3(transform.position.x, 1f, transform.position.z);
        for (var i = 0; i < 24; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                var warrior = hit.collider.GetComponent<Warrior>();
                var creature = hit.collider.GetComponent<Creature>();

                if (creature != null && warrior != null && creature.CreatureType == victimType)
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    return warrior.transform;
                }
                else
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(pos, direction * aggroRadius, Color.white);
            }
            direction = stepAngle * direction;
        }
        return null;
    }
    public delegate void WarriorDidAttackEnemy(float damage);

    public void GiveDamage(Warrior target, Weapon weapon)
    {
        float damage = (GetStats().attack - target.GetStats().defense) * DeathMatchWeapon.i.Damage(weapon);
        if (damage <= 0) damage = 1;
        target.ReceiveDamage(damage, this);
    }

    public void ReceiveDamage(float damage, Warrior attacker)
    {
        bool isCriticalHit = UnityEngine.Random.Range(0, 100) < 30;
        DamagePopup.Create(transform, damage, isCriticalHit);
        warriorHealth.ModifyHealth(-damage);

        if (warriorHealth.currentHealth <= 0)
        {
            isDead = true;
            attacker.level += level;
            attacker.warriorHealth.SetLevel(attacker.level);
            attacker.warriorHealth.SetCurrentHealth(attacker.GetStats().maxHealth);
            if (attacker.GetComponent<Creature>().CreatureType == CreatureType.Player)
            {
                TagResolver.i.deathMatch.IncrementPlayerKills();
            }
        }
        if (haveBeenAttacked != null)
        {
            Vector3 fromPosition = attacker.transform.position;
            Vector3 toPosition = transform.position;
            Vector3 dir = (toPosition - fromPosition).normalized;
            haveBeenAttacked(new WarriorAttacked(damage, dir, attacker));
        }
    }
}