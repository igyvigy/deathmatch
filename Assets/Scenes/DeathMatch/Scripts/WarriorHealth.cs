using System;
using UnityEngine;

public class WarriorHealth : MonoBehaviour
{
    public event Action<WarriorHealth> OnHealthAdded = delegate { };
    public event Action<WarriorHealth> OnHealthRemoved = delegate { };

    public event Action<float> OnHealthPercentChaged = delegate { };
    public event Action<int> OnLevelChaged = delegate { };
    private Warrior warrior;
    public float currentHealth { get; private set; }
    private void Start()
    {
        warrior = GetComponentInParent<Warrior>();
        currentHealth = warrior.GetStats().maxHealth;
        SetLevel(1);
    }
    public void Show()
    {
        OnHealthAdded(this);
    }

    public void Hide()
    {
        OnHealthRemoved(this);
    }

    public void ModifyHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth > warrior.GetStats().maxHealth)
        {
            currentHealth = warrior.GetStats().maxHealth;
        }
        else if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        if (currentHealth == 0)
        {
            OnHealthRemoved(this);
        }
        else if (currentHealth > 0)
        {
            OnHealthAdded(this);
        }
        float healthPercent = currentHealth / warrior.GetStats().maxHealth;
        OnHealthPercentChaged(healthPercent);
    }
    public void SetCurrentHealth(float value)
    {
        currentHealth = value;
        ModifyHealth(0);
    }

    public void SetLevel(int level)
    {
        OnLevelChaged(level);
    }
}