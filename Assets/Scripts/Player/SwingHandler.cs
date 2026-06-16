using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SwingHandler : MonoBehaviour
{
    [SerializeField] private SwingDataSO data;
    [SerializeField] private DroneController drone;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private GameEventSO onSwingStart;
    [SerializeField] private GameEventSO onSwingEnd;

    public bool IsSwinging { get; private set; }

    private Rigidbody rb;
    private InputSystem_Actions inputActions;

    private Vector3 anchorPos;
    private float ropeLength;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rope != null)
            rope.enabled = false;
    }

    void Start()
    {
        inputActions = GetComponent<Player_Movement_Script>().Input;
    }

    void Update()
    {
        bool pressed = inputActions.Player.Swing.IsPressed();

        if (pressed && !IsSwinging)
            TryStartSwing();
        else if (!pressed && IsSwinging)
            Release();
    }

    private void TryStartSwing()
    {
        // Anchor on the player's own plane.
        Vector3 dronePos = drone.transform.position;
        dronePos.z = rb.position.z;

        float distance = Vector3.Distance(rb.position, dronePos);
        if (distance > data.maxAttachDistance)
            return;

        anchorPos = dronePos;
        ropeLength = Mathf.Max(distance, data.minRopeLength);
        IsSwinging = true;

        drone.LockAt(anchorPos);

        if (rope != null)
            rope.enabled = true;

        onSwingStart?.Raise();
    }

    void FixedUpdate()
    {
        if (!IsSwinging)
            return;

        // Air control influence before the constraint re-projects velocity onto the tangent.
        float moveX = inputActions.Player.Move.ReadValue<Vector2>().x;
        rb.AddForce(new Vector3(moveX * data.airControlForce, 0f, 0f));

        // Rigid fixed-length distance constraint.
        Vector3 toAnchor = anchorPos - rb.position;
        float dist = toAnchor.magnitude;
        if (dist > ropeLength && dist > 0f)
        {
            Vector3 dir = toAnchor / dist;
            rb.position = anchorPos - dir * ropeLength;     // clamp back onto the circle

            float radial = Vector3.Dot(rb.linearVelocity, -dir);
            if (radial > 0f)                                // remove only the outward velocity
                rb.linearVelocity += dir * radial;
        }

        Vector3 v = rb.linearVelocity;
        v.z = 0f;                                           // no depth drift (FreezePositionZ keeps the plane)
        rb.linearVelocity = v;
    }

    private void Release()
    {
        IsSwinging = false;
        drone.Unlock();

        if (rope != null)
            rope.enabled = false;

        // Preserve tangential momentum; just boost and clamp.
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity * data.releaseBoost, data.maxReleaseSpeed);

        onSwingEnd?.Raise();
    }

    void LateUpdate()
    {
        if (!IsSwinging || rope == null)
            return;

        rope.positionCount = 2;
        rope.SetPosition(0, transform.position);
        rope.SetPosition(1, anchorPos);
    }
}
