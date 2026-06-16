using UnityEngine;

[CreateAssetMenu(fileName = "DroneData", menuName = "SO/DroneData")]
public class DroneDataSO : ScriptableObject
{
    [Header("Follow")]
    public Vector2 droneOffset = new Vector2(1.5f, 3f);
    public float droneFollowSmoothTime = 0.15f;
    public float verticalSmoothTime = 0.45f;

    [Header("Ledge / gap detection")]
    public LayerMask groundMask;
    public float ledgeProbeAhead = 2f;
    public float ledgeProbeDepth = 2f;
    public Vector2 gapAnchorOffset = new Vector2(6f, 5f);
}
