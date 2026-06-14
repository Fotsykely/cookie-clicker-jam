using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public float speed = 10f;
    public float jumpForce = 10f;
}
