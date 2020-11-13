using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Public variables
    public enum AIStates
    {
        Seaching,
        Attacking,
        Dead
    };

    [Header("AI Movement/Health Settings")]
    [Range(0, 100)] public int aiHealth = 100;
    [Range(0, 15)] public float moveSpeed;
    public bool isMovingRight;
    [Range(0, 30)] public float aiDeadLifetime;

    [Header("AI State Settings")]
    [Range(0, 20)] public float aiSightRange;
    [Range(0, 10)] public float aiAttackDelay;
    public AIStates aiState;

    [Header("AI Animation Settings")]
    public Sprite IdleAnimation;
    public Sprite WalkAnimation;
    public Sprite DeadAnimation;

    [Header("Reference GameObjects")]
    public Transform rayPoint;
    public Transform playerPos;
    public GameObject aiProjectile;
    public Transform aiProjectileSpawnPoint;

    // Private variables
    private Rigidbody2D rb;
    private float playerDist;
    private float curTime;
    private float nextFireTime;

    // Start is called before the first frame update
    void Start()
    {
        // Set initial AI State
        aiState = AIStates.Seaching;

        // Set true
        isMovingRight = true;

        // Set initial health
        aiHealth = 100;

        // Fetch RigidBody2D
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDist = Vector2.Distance(transform.position, playerPos.position);

        // Do certain actions based on AI state
        switch(aiState)
        {

            case AIStates.Seaching:
                // Move the AI right
                transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);

                // Send a raycast into the ground
                RaycastHit2D groundInfo = Physics2D.Raycast(rayPoint.position, Vector2.down);

                // If the AI does not detect ground
                if(groundInfo.collider == false)
                {
                    if(isMovingRight == true)
                    {
                        // Make it so AI moves left
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        aiProjectileSpawnPoint.eulerAngles = new Vector3(0, -180, 0);
                        isMovingRight = false;
                    } else
                    {
                        // Reset the values
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        aiProjectileSpawnPoint.eulerAngles = new Vector3(0, 0, 0);
                        isMovingRight = true;
                    }
                }

                // If the player is in range
                if (playerDist < aiSightRange)
                {
                    aiState = AIStates.Attacking;
                }

                break;

            case AIStates.Attacking:
                // Check if player has escaped from AI
                if(playerDist > aiSightRange)
                {
                    aiState = AIStates.Seaching;
                }

                // Move towards the player
                groundInfo = Physics2D.Raycast(rayPoint.position, Vector2.down);

                // Make sure the AI doesn't walk of the edge
                if(groundInfo.collider != false)
                {
                    transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
                }

                // Check if player is in front
                RaycastHit2D playerInfo = Physics2D.Raycast(transform.position, Vector2.right, aiSightRange);

                if(playerInfo.collider == false)
                {
                    transform.eulerAngles = new Vector3(0, transform.rotation.y + 180f, 0);
                    aiProjectileSpawnPoint.eulerAngles = new Vector3(0, transform.rotation.y + 180f, 0);
                }

                // Attack player
                FireProjectile();

                break;

            case AIStates.Dead:
                // Set AI sprite to dead AI sprite

                // Delete after time
                curTime += Time.deltaTime;

                if(curTime > aiDeadLifetime)
                {
                    Destroy(gameObject);
                }

                break;
        }
    }

    // Fire projectile at player
    private void FireProjectile()
    {
        if(nextFireTime < Time.time)
        {
            if(isMovingRight == false)
            {
                Instantiate(aiProjectile, aiProjectileSpawnPoint.position, Quaternion.Euler(0, 180, 0));
            } else
            {
                Instantiate(aiProjectile, aiProjectileSpawnPoint.position, Quaternion.identity);
            }

            nextFireTime = Time.time + aiAttackDelay;
        }
    }

    // On collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If collision was with a projectile
        if(collision.gameObject.tag == "Projectile")
        {
            // Destroy the RigidBody2D
            Destroy(rb);

            // Set the enemies state to dead
            aiState = AIStates.Dead;

            // Destroy projectile
            Destroy(collision.gameObject);
        }
    }
}
