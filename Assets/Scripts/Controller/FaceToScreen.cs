using UnityEngine;

public class FaceToScreen : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool lockYAxis = true;

    private void LateUpdate()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (cameraTransform == null)
        {
            return;
        }

        if (lockYAxis)
        {
            Vector3 directionToCamera = cameraTransform.position - transform.position;
            directionToCamera.y = 0f;

            if (directionToCamera.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(
                transform.position - cameraTransform.position,
                Vector3.up);
        }
    }
}