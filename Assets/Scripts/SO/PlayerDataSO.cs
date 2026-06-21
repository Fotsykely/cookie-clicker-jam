using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public float speed = 10f;
    public float jumpForce = 10f;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public float turnSpeed = 720f; // degrees/second the model rotates to face its movement direction
}
