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
    public int aiHealth = 100;
    [Range(0, 15)] public float moveSpeed;
    public bool isMovingRight;
    [Range(0, 30)] public float aiDeadLifetime;

    [Header("AI State Settings")]
    [Range(0, 20)] public float aiSightRange;
    public AIStates aiState;

    [Header("AI Animation Settings")]
    public Sprite IdleAnimation;
    public Sprite WalkAnimation;
    public Sprite DeadAnimation;

    [Header("Reference GameObjects")]
    public Transform rayPoint;
    public Transform playerPos;

    // Private variables
    private float playerDist;
    private float curTime;

    // Start is called before the first frame update
    void Start()
    {
        // Set initial AI State
        aiState = AIStates.Seaching;

        // Set true
        isMovingRight = true;

        // Set initial health
        aiHealth = 100;
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
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        isMovingRight = false;
                    } else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
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

                transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);

                // Attack player
                break;

            case AIStates.Dead:
                // Set AI sprite to dead AI sprite

                gameObject.GetComponent<SpriteRenderer>().sprite = DeadAnimation;

                // Delete after time
                curTime += Time.deltaTime;

                if(curTime >= aiDeadLifetime)
                {
                    Destroy(gameObject);
                }

                break;
        }
    }
}
