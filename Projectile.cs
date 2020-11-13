using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Public variables
    public float projectileForce;
    public float projectileLifetime;

    // Private variables
    private float curTime;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // Find the RigidBody2D
        rb = gameObject.GetComponent<Rigidbody2D>();

        // Add force to the RigidBody2D
        rb.AddForce(Vector2.right * projectileForce);
    }

    void Update()
    {
        // Increment time
        curTime += Time.deltaTime;
        
        // If current time is bigger then the life time
        if(curTime >= projectileLifetime)
        {
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
