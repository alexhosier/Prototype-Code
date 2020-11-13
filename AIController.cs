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

    [Header("AI Settings")]
    public int aiHealth = 100;
    [Range(0, 15)] public float moveSpeed;
    [Range(0, 15)] public float aiSightRange;
    public bool isMovingRight;
    public AIStates aiState;

    [Header("Reference GameObjects")]
    public Transform rayPoint;
    public Transform playerPos;

    // Private variables

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
        // Do certain actions based on AI state
        switch(aiState)
        {
            case AIStates.Seaching:
                // Move the AI right
                transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);

                // Send a raycast into the ground
                RaycastHit2D raycast = Physics2D.Raycast(rayPoint.position, Vector2.down);

                // If the AI does not detect ground
                if(raycast.collider == null && isMovingRight == true)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    isMovingRight = false;
                } else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    isMovingRight = true;
                }

                // If the player is in range
                if(Vector3.Distance(transform.position, playerPos.position) < aiSightRange)
                {
                    aiState = AIStates.Attacking;
                }

                break;

            case AIStates.Attacking:
                // Check if player has escaped from AI
                if(Vector3.Distance(transform.position, playerPos.position) > aiSightRange)
                {
                    aiState = AIStates.Seaching;
                }
                break;

            case AIStates.Dead:
                Debug.Log("AI Is dead");
                break;
        }
    }
}
