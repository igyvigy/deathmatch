using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None,
    TwoHandedSword,
    Spear,
    Axe,
    Bow,
    Arbalet,
    Staff,
    ShieldSword,
    Dagger,
    Wand,
    ThrowingKnifes,
    DualSwords,
    SummonSphere
}

public class DeathMatchPlayer : MonoBehaviour
{

    private Warrior warrior;
    private Creature creature;

    void Start()
    {
        creature = GetComponent<Creature>();
        warrior = GetComponent<Warrior>();
    }
    public Warrior GetWarrior()
    {
        return warrior;
    }
    public Creature GetCreature()
    {
        return creature;
    }
    public float GetCurrentHealth()
    {
        if (warrior == null) return 0;
        return warrior.warriorHealth.currentHealth;
    }
    public float GetMaxHealth()
    {
        if (warrior == null) return 0;
        return warrior.GetStats().maxHealth;
    }

}
