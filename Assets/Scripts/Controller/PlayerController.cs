using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("移动")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private bool normalizeDiagonalInput = true;
    [SerializeField] private bool enableEightDirectionSnap = false;
    [SerializeField] private bool isMovementEnable = true;

    [Header("朝向")]
    [SerializeField] private bool rotateTowardMovementDirection = true;
    [SerializeField] private float rotationSpeedDegrees = 720f;

    [Header("引用")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController;
    private float verticalVelocity;

    public GameObject UI;
    private bool isOpen;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (cameraTransform == null)
        {
            return;
        }
        if (!isMovementEnable)
        {
            HandleDialogueInput();
            return;
        }

        Vector2 inputAxis = ReadMovementInput();
        Vector3 moveDirection = BuildCameraRelativeMoveDirection(inputAxis);

        if (enableEightDirectionSnap)
        {
            moveDirection = SnapToEightDirections(moveDirection);
        }

        Vector3 horizontalVelocity = moveDirection * moveSpeed;

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 motion = horizontalVelocity;
        motion.y = verticalVelocity;

        characterController.Move(motion * Time.deltaTime);

        if (rotateTowardMovementDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            RotateTowardDirection(moveDirection);
        }

        HandleDialogueInput();
    }

    private Vector2 ReadMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 inputAxis = new Vector2(horizontal, vertical);

        if (normalizeDiagonalInput && inputAxis.sqrMagnitude > 1f)
        {
            inputAxis.Normalize();
        }

        return inputAxis;
    }

    private Vector3 BuildCameraRelativeMoveDirection(Vector2 inputAxis)
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 moveDirection =
            cameraForward * inputAxis.y +
            cameraRight * inputAxis.x;

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        return moveDirection;
    }

    private Vector3 SnapToEightDirections(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude < 0.001f)
        {
            return Vector3.zero;
        }

        float angleDegrees = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float snappedAngleDegrees = Mathf.Round(angleDegrees / 45f) * 45f;
        float snappedAngleRadians = snappedAngleDegrees * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Sin(snappedAngleRadians),
            0f,
            Mathf.Cos(snappedAngleRadians));
    }

    private void RotateTowardDirection(Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeedDegrees * Time.deltaTime);
    }

    public void SetMovementEnabled(bool enabled)
    {
        isMovementEnable = enabled;
    }
    private void HandleDialogueInput()
    {
        if (UIManager.Instance == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && UIManager.Instance.IsDialogueOpen)
        {
            UIManager.Instance.CloseDialogue();
            return;
        }
        if (!Input.GetKeyDown(KeyCode.E))
        {
            return;
        }
        if (!NPCController.IsPlayerInRange)
        {
            return;
        }
        UIManager.Instance.ToggleDialogue();
    }
}