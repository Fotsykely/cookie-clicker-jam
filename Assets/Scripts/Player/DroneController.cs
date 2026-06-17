using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private DroneDataSO data;
    [SerializeField] private Transform target;

    private bool locked;
    private float facing = 1f;          // +1 right, -1 left (last non-zero move direction)
    private float lastTargetX;
    private float velX, velY;           // separate SmoothDamp velocities for horizontal / vertical

    void Start()
    {
        lastTargetX = target.position.x;
    }

    void LateUpdate()
    {
        if (locked)
            return;

        UpdateFacing();

        // Forward & higher over a gap when the player is at a ledge, normal hover otherwise.
        Vector2 off = AtLedge() ? data.gapAnchorOffset : data.droneOffset;
        Vector3 desired = target.position + new Vector3(facing * off.x, off.y, 0f);

        // Smooth horizontal and vertical separately: reactive on X, gentle on Y (no jump spike).
        float x = Mathf.SmoothDamp(transform.position.x, desired.x, ref velX, data.droneFollowSmoothTime);
        float y = Mathf.SmoothDamp(transform.position.y, desired.y, ref velY, data.verticalSmoothTime);
        transform.position = new Vector3(x, y, target.position.z);  // stay on the player's plane
    }

    private void UpdateFacing()
    {
        float dx = target.position.x - lastTargetX;
        if (Mathf.Abs(dx) > 0.001f)
            facing = Mathf.Sign(dx);
        lastTargetX = target.position.x;
    }

    // True when there is ground under the player but none ahead in the facing direction.
    private bool AtLedge()
    {
        Vector3 up = Vector3.up * 0.1f;
        bool groundHere = Physics.Raycast(target.position + up, Vector3.down, data.ledgeProbeDepth, data.groundMask);
        bool groundAhead = Physics.Raycast(target.position + up + Vector3.right * (facing * data.ledgeProbeAhead),
                                           Vector3.down, data.ledgeProbeDepth, data.groundMask);
        return groundHere && !groundAhead;
    }

    // Freeze the drone in place so it acts as a fixed swing pivot.
    public void LockAt(Vector3 worldPos)
    {
        transform.position = worldPos;
        velX = velY = 0f;
        locked = true;
    }

    // Resume following the player.
    public void Unlock()
    {
        locked = false;
        velX = velY = 0f;
    }
}
