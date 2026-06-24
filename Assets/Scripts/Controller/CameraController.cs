using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("跟随目标")]
    [SerializeField] private Transform followTarget;

    [Header("HD-2D 机位")]
    [SerializeField] private float cameraPitchDegrees = 45f;
    [SerializeField] private float cameraYawDegrees = 45f;
    [SerializeField] private float followDistance = 18f;
    [SerializeField] private float heightOffset = 0f;
    [SerializeField] private float smoothTime = 0.15f;

    [Header("投影（可选）")]
    [SerializeField] private bool useOrthographicProjection = false;
    [SerializeField] private float orthographicSize = 8f;

    private Camera cameraComponent;
    private Vector3 positionVelocity;
    private Quaternion fixedCameraRotation;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();

        if (followTarget == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                followTarget = playerObject.transform;
            }
        }

        fixedCameraRotation = Quaternion.Euler(cameraPitchDegrees, cameraYawDegrees, 0f);
        ApplyProjectionSettings();
    }

    private void LateUpdate()
    {
        if (followTarget == null)
        {
            return;
        }

        transform.rotation = fixedCameraRotation;

        Vector3 cameraBackOffset = fixedCameraRotation * Vector3.back * followDistance;
        Vector3 desiredPosition = followTarget.position + cameraBackOffset;
        desiredPosition.y += heightOffset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref positionVelocity,
            smoothTime);
    }

    private void ApplyProjectionSettings()
    {
        if (cameraComponent == null)
        {
            return;
        }

        cameraComponent.orthographic = useOrthographicProjection;

        if (useOrthographicProjection)
        {
            cameraComponent.orthographicSize = orthographicSize;
        }
    }

    private void OnValidate()
    {
        if (cameraComponent == null)
        {
            cameraComponent = GetComponent<Camera>();
        }

        fixedCameraRotation = Quaternion.Euler(cameraPitchDegrees, cameraYawDegrees, 0f);
        ApplyProjectionSettings();
    }
}