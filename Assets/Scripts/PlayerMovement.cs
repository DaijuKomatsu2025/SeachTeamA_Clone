using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public float speed = 5f;

    private CharacterController controller;
    private InputAction _move;
    private Vector2 moveInput;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        var input = GetComponent<PlayerInput>();
        input.currentActionMap.Enable();

        _move = input.currentActionMap.FindAction("Move");
    }

    void Update()
    {
        moveInput = _move.ReadValue<Vector2>();

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        controller.Move(moveDirection * speed * Time.deltaTime);
    }
}
