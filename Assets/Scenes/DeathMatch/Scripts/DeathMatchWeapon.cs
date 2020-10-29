using UnityEngine;
using DeathMatchCharacterAnims;


[SerializeField]
public class WeaponConfigItem
{
    public string key;
    public float value;

    public WeaponConfigItem(string key, float value)
    {
        this.key = key;
        this.value = value;
    }
}

[SerializeField]
public class WeaponConfig
{
    public string name;
    public WeaponConfigItem[] items;

    public WeaponConfig(string name, WeaponConfigItem[] items)
    {
        this.name = name;
        this.items = items;
    }
}

public class DeathMatchWeapon : MonoBehaviour
{

    public WeaponConfig[] configs =
        new WeaponConfig[2] {
            new WeaponConfig("unarmed", new WeaponConfigItem[2] {
                new WeaponConfigItem("range", 1.2f),
                new WeaponConfigItem("damage", 1.0f)
                }),
            new WeaponConfig("two_hand_sword", new WeaponConfigItem[2] {
                new WeaponConfigItem("range", 2.0f),
                new WeaponConfigItem("damage", 2.0f)
                }),
        };

    private static DeathMatchWeapon _i;

    public static DeathMatchWeapon i
    {
        get
        {
            if (_i == null) _i = GameObject.FindGameObjectWithTag("death_match").GetComponent<DeathMatchWeapon>();
            return _i;
        }
    }

    public float AttackRange(Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.UNARMED:
                return configs[0].items[0].value;
            case Weapon.TWOHANDSWORD:
                return configs[1].items[0].value;
            default: return 1f;
        }
    }
    public float Damage(Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.UNARMED:
                return configs[0].items[1].value;
            case Weapon.TWOHANDSWORD:
                return configs[1].items[1].value;
            default: return 1f;
        }
    }
}
