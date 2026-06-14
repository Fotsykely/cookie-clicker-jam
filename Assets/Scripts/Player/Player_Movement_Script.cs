using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement_Script : MonoBehaviour
{
    [SerializeField] private PlayerDataSO data;
    [SerializeField] private Transform groundCheck;

    private Rigidbody rb;
    private bool grounded;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
        inputActions.Player.Jump.performed += _ => Jump();
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, data.groundDistance, data.groundMask);
        float move = inputActions.Player.Move.ReadValue<Vector2>().x;
        rb.linearVelocity = new Vector3(move * data.speed, rb.linearVelocity.y, 0);
    }

    private void Jump()
    {
        if (grounded)
            rb.AddForce(Vector3.up * data.jumpForce, ForceMode.Impulse);
    }
}
