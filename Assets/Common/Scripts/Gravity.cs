using UnityEngine;
using System.Collections;
using DeathMatchCharacterAnims;

/// <summary>
/// Rotates a this transform to align it towards the target transform's position.
/// </summary>
public class Gravity : MonoBehaviour
{
    [SerializeField] Transform planet;

    void Update()
    {
        Vector3 dir = (transform.position - planet.position).normalized;
        GetComponent<DeathMatchPlayerMovementController>().RotateGravity(dir);
        transform.rotation = Quaternion.FromToRotation(transform.up, dir) * transform.rotation;
    }
}