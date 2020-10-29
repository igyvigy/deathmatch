using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage;

    [SerializeField]
    private TextMeshProUGUI levelLabel;

    [SerializeField]
    private float updateSpeedSeconds = 0.5f;
    [SerializeField]
    private float positionOffset;

    private WarriorHealth health;

    public void SetHealth(WarriorHealth health)
    {
        this.health = health;
        this.health.OnHealthPercentChaged += HandleHealthPercentChanged;
        this.health.OnLevelChaged += SetLevel;
    }

    public void SetLevel(int level)
    {
        levelLabel.SetText(level.ToString());
    }
    private void HandleHealthPercentChanged(float healthPercent)
    {
        StartCoroutine(ChangeToPct(healthPercent));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        foregroundImage.fillAmount = pct;
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(health.transform.position + Vector3.up * positionOffset);
    }

    private void OnDestroy()
    {
        health.OnHealthPercentChaged -= HandleHealthPercentChanged;
    }
}
