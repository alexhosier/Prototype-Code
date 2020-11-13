using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    // Public variables
    public enum PickupTypes
    {
        None,
        Key,
        Health,
        Secret
    };

    [Header("Pickup Settings")]
    public PickupTypes pickupType;
    public int healthToAdd;

    // Private variables
    private GameManager gameManager;
    private PlayerController playerController;

    // At the start
    void Start()
    {
        // Find the game manager script
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Find the player controller script
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // On collision with another object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision was with the player
        if(collision.gameObject.tag == "Player")
        {
            // Action based on the type of pickup
            switch(pickupType)
            {
                case PickupTypes.Key:
                    gameManager.hasKey = true;
                    break;

                case PickupTypes.Health:
                    playerController.UpdateHealth(healthToAdd);
                    break;

                case PickupTypes.Secret:
                    Debug.Log("Secret has been found!");
                    break;

                default:
                    Debug.LogError("Unkown pickup type");
                    break;
            }
        }
    }
}
