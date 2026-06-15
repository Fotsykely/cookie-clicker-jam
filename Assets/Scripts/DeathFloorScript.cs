using UnityEngine;

public class DeathFloorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Death();
        }
    }

    public void Death()
    {
        transform.position = new Vector3(0,0,0);
    }
}
