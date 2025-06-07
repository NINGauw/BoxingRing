using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OneVsManyManager : MonoBehaviour
{
    static public OneVsManyManager Instance { get; private set; }
    [Header("Enemy Spawn Settings")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public int totalEnemiesInLevel = 3;
    public float timeBetweenEnemies = 2.5f;

    [Header("References")]
    public PlayerController playerController;
    public GameObject winPanel;
    public GameObject losePanel;
    private int currentEnemyDefeated = 0;
    private EnemyController currentEnemy;

    private void Start()
    {
        Instance = this;
        currentEnemyDefeated = 0;
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        SpawnNextEnemy();
    }

    public void OnEnemyDefeated()
    {
        currentEnemyDefeated++;
        if (currentEnemyDefeated < totalEnemiesInLevel)
        {
            StartCoroutine(SpawnNextEnemyWithDelay());
        }
        else
        {
            LevelComplete();
        }
    }

    IEnumerator SpawnNextEnemyWithDelay()
    {
        yield return new WaitForSeconds(timeBetweenEnemies);
        SpawnNextEnemy();
        EnemyHealthUI.Instance.OnSpawnedEnemy();
    }

    private void SpawnNextEnemy()
    {
        GameObject enemyGO = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        currentEnemy = enemyGO.GetComponent<EnemyController>();

        playerController.currentEnemy = currentEnemy;

        EnemyAIController ai = enemyGO.GetComponent<EnemyAIController>();
        if (ai != null)
        {
            ai.playerTarget = playerController;
        }
    }
    private void LevelComplete()
    {
        winPanel.SetActive(true);
    }
    public int GetCurrentEnemyDefeated()
    {
        return currentEnemyDefeated;
    }
    public void LoseRound()
    {
        losePanel.SetActive(true);
    }
}
