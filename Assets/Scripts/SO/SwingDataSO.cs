using UnityEngine;

[CreateAssetMenu(fileName = "SwingData", menuName = "SO/SwingData")]
public class SwingDataSO : ScriptableObject
{
    [Header("Rope")]
    public float maxAttachDistance = 8f;
    public float minRopeLength = 1.5f;

    [Header("Swing")]
    public float airControlForce = 15f;
    public float releaseBoost = 1.1f;
    public float maxReleaseSpeed = 25f;
}
