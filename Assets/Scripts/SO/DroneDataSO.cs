using UnityEngine;

[CreateAssetMenu(fileName = "DroneData", menuName = "SO/DroneData")]
public class DroneDataSO : ScriptableObject
{
    [Header("Follow")]
    public Vector2 droneOffset = new Vector2(1.5f, 3f);
    public float droneFollowSmoothTime = 0.15f;
}
