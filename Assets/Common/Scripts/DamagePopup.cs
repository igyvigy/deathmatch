using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Transform target, float damage, bool isCriticalHit)
    {
        Window_Pointer pointerUI = TagResolver.i.deathMatch.pointerUI;
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, target.position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup((int)damage, isCriticalHit);
        damagePopup.transform.SetParent(pointerUI.transform);
        damagePopup.transform.position = target.position + new Vector3(0, 1, -1) * 1f;
        damagePopup.transform.LookAt(Camera.main.transform);
        damagePopup.transform.Rotate(0, 180, 0);
        return damagePopup;
    }

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private static VertexSortingOrder sortingOrder;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        if (isCriticalHit)
        {
            textMesh.SetText(damageAmount.ToString() + "!");
            textMesh.fontSize = 10;
            textColor = UtilsClass.GetColorFromString("F55442");
        }
        else
        {
            textMesh.SetText(damageAmount.ToString());
            textMesh.fontSize = 5;
            textColor = UtilsClass.GetColorFromString("f5bc42");
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.geometrySortingOrder = sortingOrder;

        moveVector = new Vector3(UnityEngine.Random.Range(-1f, 1f), 1);
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector.y -= 6f * Time.deltaTime;

        disappearTimer -= Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            // first half of popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // second half
            float increaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * increaseScaleAmount * Time.deltaTime;
        }

        if (disappearTimer < 0)
        {
            // start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

}

