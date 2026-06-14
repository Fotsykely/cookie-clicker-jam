using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_Script : MonoBehaviour
{
    public float speed = 10; 
    public Rigidbody rb;
    public bool grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector3(Input.GetAxis("Horizontal") * speed,rb.linearVelocity.y,0);
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(0,10,0, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
