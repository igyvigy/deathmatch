﻿using System.Collections.Generic;
using UnityEngine;

public class Window_HealthBar : MonoBehaviour
{
    private Dictionary<WarriorHealth, HealthBar> healthBars = new Dictionary<WarriorHealth, HealthBar>();

    public void SubscribeOnWarriorHealth(WarriorHealth health)
    {
        health.OnHealthAdded += AddHealthBar;
        health.OnHealthRemoved += RemoveHealthBar;
    }

    private void AddHealthBar(WarriorHealth health)
    {
        if (!healthBars.ContainsKey(health))
        {
            var healthBar = Instantiate(GameAssets.i.pfHealthBar, transform);
            healthBars.Add(health, healthBar);
            healthBar.SetHealth(health);
        }
    }

    private void RemoveHealthBar(WarriorHealth health)
    {
        if (healthBars.ContainsKey(health))
        {
            Destroy(healthBars[health].gameObject);
            healthBars.Remove(health);
        }
    }
}
