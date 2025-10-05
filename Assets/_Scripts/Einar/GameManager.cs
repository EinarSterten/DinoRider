using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    private int enemiesRemaining = 4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (IsServer)
        {
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            var spawnPosition = spawnPoints[i].position;
            var enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemyObject.GetComponent<NetworkObject>().Spawn();
        }
    }

    public void OnEnemyDespawned()
    {
        Debug.Log("Enemy despawned, GameManager notified.");
        enemiesRemaining--;
        Debug.Log("Enemies remaining: " + enemiesRemaining);
        if (enemiesRemaining <= 0)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        Debug.Log("All enemies defeated! You win!");
    }
}