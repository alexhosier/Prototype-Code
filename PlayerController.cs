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
    [Range(0, 100)] public int health;

    [Header("Animation Setttings")]
    public Animator animator;

    [Header("Reference GameObjects")]
    public GameObject projectile;
    public Transform projectileSpawn;
    public GameManager gameManager;
    public Transform door;

    // Private variables
    private Rigidbody2D rb;
    private float distToDoor;
    private bool hasKey = false;

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

        // If the door exists
        if(door != null)
        {
            // Get the distance
            distToDoor = Vector2.Distance(transform.position, door.position);
        }

        // Check if the player is close to the door
        if(distToDoor < 5)
        {
            // Check if player presses interact and haskey
            if(Input.GetKeyDown(KeyCode.E) && hasKey)
            {
                // Load the next scene
                gameManager.LoadScene(1);
            }
        }
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

        // Animate player based on movement
        if (moveX > 0 || moveX < 0)
        {
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }

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

    // Fire projectile method
    private void FireProjectile()
    {
        // If player press mouse 1
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject projectileToFire = projectile;

            projectileToFire.GetComponent<Projectile>().projectileDirection = Projectile.FlightDirections.Right;

            Instantiate(projectileToFire, projectileSpawn.position, Quaternion.identity);
        }
    }

    // On collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            // If player touches floor
            case "Floor":

                // Allow the player to jump again
                isGrounded = true;

                break;

            // If player touches enemy
            case "Enemy":

                // Minus 10 health
                health -= 10;

                break;

            // If player touches pickup
            case "Pickup":

                // Fetch the pickup controller
                PickupController pickup = collision.gameObject.GetComponent<PickupController>();

                // Switch between the types of pickups
                switch(pickup.pickupType)
                {
                    // If player touches the health pickup
                    case PickupController.PickupTypes.Health:

                        // Increment the health based on the value in pickup controller
                        health += pickup.healthToAdd;

                        // Delete the health pickup
                        Destroy(collision.gameObject);

                        break;

                    // If player touches the key
                    case PickupController.PickupTypes.Key:

                        // Set the value to true
                        hasKey = true;
                        gameManager.hasKey = true;

                        // Delete the key
                        Destroy(collision.gameObject);

                        break;

                    // If player touches a secret
                    case PickupController.PickupTypes.Secret:

                        // DEBUG
                        Debug.Log("Player has pickuped a secret!");

                        // Destroy the secret
                        Destroy(collision.gameObject);

                        break;
                }

                break;

            // If player collides with projectile
            case "Projectile":
                // Delete the projectile
                Destroy(collision.gameObject);

                // Restart scene
                gameManager.LoadScene(0);

                break;

            case "Door":

                Debug.Log("The player is touching door");

                break;

            // If tag is unkown (ERROR)
            default:

                Debug.Log("Unkown GameObject tag");

                break;
        }
    }
}
