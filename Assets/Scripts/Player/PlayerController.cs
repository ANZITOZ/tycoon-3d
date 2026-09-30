using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float gravity = -25f;

    [Header("References")]
    [SerializeField] private MobileJoystick joystick;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform visual;

    private CharacterController characterController;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector2 input = joystick != null ? joystick.InputVector : Vector2.zero;

        Vector3 movement = GetCameraRelativeMovement(input);

        if (movement.sqrMagnitude > 0.001f)
        {
            movement.Normalize();

            if (visual != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                visual.rotation = Quaternion.Slerp(
                    visual.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
            }
        }

        if (characterController.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = movement * moveSpeed;
        velocity.y = verticalVelocity;

        characterController.Move(velocity * Time.deltaTime);
    }

    private Vector3 GetCameraRelativeMovement(Vector2 input)
    {
        if (input.sqrMagnitude <= 0.001f)
            return Vector3.zero;

        if (cameraTransform == null)
            return new Vector3(input.x, 0f, input.y);

        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        return right * input.x + forward * input.y;
    }

    public void SetJoystick(MobileJoystick newJoystick)
    {
        joystick = newJoystick;
    }
}
