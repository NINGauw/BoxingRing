using UnityEngine;
using System;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    static public LevelManager Instance { get; private set; }
    [Header("Enemy")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    [Header("Level")]
    public int currentLevel = 1;
    public int maxLevel = 10;
    private LevelData currentLevelData;

    private int enemiesDefeated = 0;
    private int enemiesSpawned = 0;

    [Header("Reference")]
    public PlayerController playerController;
    public PlayerUIController playerUI;
    public GameObject winPanel;
    public GameObject losePanel;
    public Button nextLevelButton;
    private EnemyController currentActiveEnemy;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        if (enemyPrefab == null)
        {
            enabled = false;
            return;
        }
        if (spawnPoint == null)
        {
            enabled = false;
            return;
        }
        if (playerController == null)
        {
            enabled = false;
            return;
        }
        SetupLevel();
    }
    void SetupLevel()
    {
        currentLevelData = GenerateLevelData(currentLevel);

    
        //Reset các para
        enemiesDefeated = 0;
        enemiesSpawned = 0;
        SpawnNextEnemy();
    }
    public void LoadNextLevel()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            winPanel.SetActive(false);
            SetupLevel();
        }
        else
        {

        }
    }

    public LevelData GenerateLevelData(int levelNumber)
    {
        LevelData data = new LevelData();

        // Số lượng Enemy
        int baseEnemies = 2; // Số enemy base
        data.totalEnemies = baseEnemies + (levelNumber / 2);

        // Máu Enemy
        int baseHealthMultiplier = 2; //Máu base 
        data.enemyHealthMultiplier = baseHealthMultiplier + (levelNumber - 1);

        // Tốc độ đánh
        float initialMinDelay = 2.5f; // Time tối thiểu base
        float initialMaxDelay = 5.0f; // Time tối đa base
        float minDelayFloor = 1.0f;   // Ngưỡng tối thiểu của min
        float maxDelayFloor = 2.0f;   // Ngưỡng tối thiểu của max
        float minReductionPerLevel = 0.15f; // Lượng giảm mỗi level
        float maxReductionPerLevel = 0.3f;  // Lượng giảm mỗi level
        data.minAttackDelay = Mathf.Max(minDelayFloor, initialMinDelay - (levelNumber - 1) * minReductionPerLevel);
        data.maxAttackDelay = Mathf.Max(maxDelayFloor, initialMaxDelay - (levelNumber - 1) * maxReductionPerLevel);

        return data;
    }

    void SpawnNextEnemy()
    {
        if (enemiesSpawned >= currentLevelData.totalEnemies)
        {
            if (currentActiveEnemy == null && enemiesDefeated >= currentLevelData.totalEnemies)
            {
                LevelComplete();
            }
            return;
        }

        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemiesSpawned++;

        currentActiveEnemy = enemyInstance.GetComponent<EnemyController>();
        EnemyAIController enemyAI = enemyInstance.GetComponent<EnemyAIController>();

        if (currentActiveEnemy != null)
        {
            currentActiveEnemy.levelManager = this;
            currentActiveEnemy.SetUpEnemyStats(currentLevelData.enemyHealthMultiplier);

            playerController.currentEnemy = currentActiveEnemy;

            EnemyHealthUI.Instance.OnSpawnedEnemy();
        }

        if (enemyAI != null)
        {
            enemyAI.playerTarget = playerController;
            enemyAI.SetUpAIStats(currentLevelData.minAttackDelay, currentLevelData.maxAttackDelay);
        }
    }

    public void OnEnemyDefeated(EnemyController defeatedEnemy)
    {
        enemiesDefeated++;
        currentActiveEnemy = null;

        if (enemiesDefeated < currentLevelData.totalEnemies)
        {
            Invoke(nameof(SpawnNextEnemy), 2.5f);
        }
        else
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        Invoke("WinRound", 2.5f);

        if (currentLevel >= maxLevel)
        {
            if (nextLevelButton != null)
            {
                nextLevelButton.gameObject.SetActive(false);
            }
        }
        else
        {
            if (nextLevelButton != null)
            {
                nextLevelButton.gameObject.SetActive(true);
            }
        }
    }

    public void LostRound()
    {
        losePanel.SetActive(true);
    }
    private void WinRound()
    {
        winPanel.SetActive(true);
    }
}