using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    public Transform target;

    public float smoothSpeed = 0.125f;
    public float cameraAdjustSpeed = 0.125f;
    public Vector3 offset;

    private GamepadInputManager inputManager;
    void Start()
    {
        inputManager = TagResolver.i.inputManager;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            target = TagResolver.i.deathMatch.players[0].transform;
        }
        float cameraFrontBack = inputManager.CameraFrontBackValue;
        float cameraUpDown = inputManager.CameraUpDownValue;
        if (cameraFrontBack != 0 || cameraUpDown != 0)
        {
            offset += new Vector3(0, cameraUpDown, cameraFrontBack) * Time.deltaTime * cameraAdjustSpeed;
        }
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        ;
    }

}