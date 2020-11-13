using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public variables
    [Header("Movement Settings")]
    [Range(0, 15)] public float moveSpeed;
    [Range(0, 350)] public float jumpForce;
    public bool isGrounded;

    [Header("Player Settings")]
    public int health;

    [Header("Reference GameObjects")]
    public GameObject projectile;

    // Private variables
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // Set the default health
        health = 100;

        // Get the RigidBody2D attached to the GameObject
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        FireProjectile();
    }

    // Update the health values
    public void UpdateHealth(int healthToAdd)
    {
        // Check if the player is not already at max health
        if (health != 100)
        {
            health += healthToAdd;
        }
    }

    // Move method
    private void Move()
    {
        // Get the axis from the input system
        float moveX = Input.GetAxis("Horizontal");

        // Move the player based on the input system
        transform.Translate(Vector2.right * Time.deltaTime * moveSpeed * moveX);
    }

    // Jump method
    private void Jump()
    {
        // If the player presses the jump axis buttons
        if(Input.GetButtonDown("Jump"))
        {
            // Check if player is on the floor
            if (isGrounded == true)
            {
                // Add force to the RigidBody2D and set variable to false
                rb.AddForce(Vector2.up * jumpForce);
                isGrounded = false;
            }
        }
    }

    // Fire projectile methiod
    private void FireProjectile()
    {
        // If player press mouse 1
        if(Input.GetButtonDown("Fire1"))
        {
            Instantiate(projectile, Vector2.right, Quaternion.identity);
        }
    }

    // On collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            // If player touches floor
            case "Floor":
                isGrounded = true;
                break;

            // If player touches enemy
            case "Enemy":
                // Damage functionality here
                break;

            // If tag is unkown (ERROR)
            default:
                Debug.LogError("Unkown GameObject tag");
                break;
        }
    }
}
