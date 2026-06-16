using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private SwingDataSO data;
    [SerializeField] private Transform target;

    private bool locked;
    private Vector3 velocity;

    void LateUpdate()
    {
        if (locked)
            return;

        Vector3 desired = target.position + (Vector3)data.droneOffset;
        desired.z = 0f;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, data.droneFollowSmoothTime);
    }

    // Freeze the drone in place so it acts as a fixed swing pivot.
    public void LockAt(Vector3 worldPos)
    {
        worldPos.z = 0f;
        transform.position = worldPos;
        velocity = Vector3.zero;
        locked = true;
    }

    // Resume following the player.
    public void Unlock()
    {
        locked = false;
        velocity = Vector3.zero;
    }
}
