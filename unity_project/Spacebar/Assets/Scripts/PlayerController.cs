using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float cameraVerticalClamp = 80f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactionLayer;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float cameraXRotation = 0f;

    private IInteractable currentInteractable;
    private bool isInteracting = false;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.Interact.canceled += OnInteractCancel;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Interact.canceled -= OnInteractCancel;
        inputActions.Player.Disable();
    }

    void Update()
    {
        HandleMovement();
        HandleCamera();
        CheckForInteractables();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCamera()
    {
        if (cameraTransform == null) return;

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        cameraXRotation -= mouseY;
        cameraXRotation = Mathf.Clamp(cameraXRotation, -cameraVerticalClamp, cameraVerticalClamp);

        cameraTransform.localRotation = Quaternion.Euler(cameraXRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void CheckForInteractables()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null && interactable != currentInteractable)
            {
                currentInteractable?.OnLookExit();
                currentInteractable = interactable;
                currentInteractable.OnLookEnter();
            }
        }
        else
        {
            currentInteractable?.OnLookExit();
            currentInteractable = null;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            isInteracting = true;
            currentInteractable.OnInteract(this.gameObject);
        }
    }

    private void OnInteractCancel(InputAction.CallbackContext context)
    {
        isInteracting = false;
        currentInteractable?.OnInteractEnd(this.gameObject);
    }

    public bool IsInteracting() => isInteracting;
    public IInteractable GetCurrentInteractable() => currentInteractable;
}
