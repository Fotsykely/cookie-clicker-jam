using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement_Script : MonoBehaviour
{
    [SerializeField] private PlayerDataSO data;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private ViewModeSO viewMode;

    private Rigidbody rb;
    private bool grounded;
    private bool topDown;
    private bool jumpRequested;
    private Camera cam;
    private InputSystem_Actions inputActions;
    private SwingHandler swing;

    // Shared input instance so other player components (e.g. SwingHandler) reuse a single Enable/Disable.
    public InputSystem_Actions Input => inputActions;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        swing = GetComponent<SwingHandler>();
        inputActions = new InputSystem_Actions();
        inputActions.Player.Jump.performed += _ => jumpRequested = true;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundCheck.position, data.groundDistance, data.groundMask);

        // While swinging, the SwingHandler drives the Rigidbody physics directly.
        if (swing != null && swing.IsSwinging)
            return;

        Vector2 move = inputActions.Player.Move.ReadValue<Vector2>();

        rb.linearVelocity = topDown
            ? TopDownVelocity(move)
            : new Vector3(move.x * data.speed, rb.linearVelocity.y, 0f);

        if (jumpRequested)
        {
            jumpRequested = false;
            if (grounded)
                rb.AddForce(Vector3.up * data.jumpForce, ForceMode.Impulse);
        }
    }

    // Calculate movement direction based on camera orientation for top-down view
    private Vector3 TopDownVelocity(Vector2 move)
    {
        if (cam == null) cam = Camera.main;
        Transform t = cam.transform;
        Vector3 fwd = Vector3.ProjectOnPlane(t.forward, Vector3.up);
        if (fwd.sqrMagnitude < 0.0001f) fwd = Vector3.ProjectOnPlane(t.up, Vector3.up); // camera looking straight down, use its up as forward
        fwd.Normalize();
        Vector3 right = Vector3.ProjectOnPlane(t.right, Vector3.up).normalized;
        Vector3 dir = fwd * move.y + right * move.x;
        return new Vector3(dir.x * data.speed, rb.linearVelocity.y, dir.z * data.speed);
    }

    // Called by CameraController's GameEventSO when the view mode changes
    public void ApplyView()
    {
        topDown = viewMode.Current == ViewMode.TopDown3D;
        // Debug.Log($"[Player] ApplyView → topDown = {topDown}");
    }
}
