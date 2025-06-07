using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManyVsManyManager : MonoBehaviour
{
    static public ManyVsManyManager Instance { get; private set; }
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
    
}
