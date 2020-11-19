using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum FlightDirections { Left, Right };

    // Public variables
    public float projectileForce;
    public float projectileLifetime;
    public FlightDirections projectileDirection;

    // Private variables
    private float curTime;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // Find the RigidBody2D
        rb = gameObject.GetComponent<Rigidbody2D>();

        switch(projectileDirection)
        {
            case FlightDirections.Left:
                rb.AddForce(Vector2.left * projectileForce);
                break;
            case FlightDirections.Right:
                rb.AddForce(Vector2.right * projectileForce);
                break;
        }
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
