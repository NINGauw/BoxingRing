using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    static public EnemyHealthUI Instance { get; private set; }
    public Slider healthSlider;
    public EnemyController enemyToTrack;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (healthSlider == null)
        {
            enabled = false;
            return;
        }
        if (LevelManager.Instance == null)
        {
            if (enemyToTrack == null)
            {
                enemyToTrack = FindObjectOfType<EnemyController>();
                if (enemyToTrack == null)
                {
                    enabled = false;
                    return;
                }
            }
            UpdateHealthBar(enemyToTrack.maxHealth, enemyToTrack.maxHealth);
        }  
        enemyToTrack.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        if (enemyToTrack != null)
        {
            enemyToTrack.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (maxHealth > 0)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
        else
        {
            healthSlider.value = 0;
        }
    }
    public void OnSpawnedEnemy()
    {
        if (enemyToTrack == null)
        {
            enemyToTrack = FindObjectOfType<EnemyController>();
        }
        enemyToTrack.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar(enemyToTrack.maxHealth, enemyToTrack.maxHealth);
    }
}