using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Progression Settings")]
    public bool hasKey;
    
    [Header("Reference GameObjects")]
    public GameObject player;
    public GameObject enemyPrefab;
    public Transform[] enemySpawns;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies();
    }

    // Load scene method
    public void LoadScene(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Spawn enemy method
    private void SpawnEnemies()
    {
        // Spawn the enemy at spawn position
        foreach (Transform pos in enemySpawns)
        {
            Instantiate(enemyPrefab, enemySpawns[Random.Range(0, enemySpawns.Length)].position, Quaternion.identity);
        }
    }
}
