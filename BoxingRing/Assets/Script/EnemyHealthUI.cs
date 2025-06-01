using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Slider healthSlider;
    public EnemyController enemyToTrack;
    void Start()
    {
        if (healthSlider == null)
        {
            enabled = false;
            return;
        }

        if (enemyToTrack == null)
        {
            enemyToTrack = FindObjectOfType<EnemyController>();
            if (enemyToTrack == null)
            {
                enabled = false;
                return;
            }
        }
        enemyToTrack.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar(enemyToTrack.maxHealth, enemyToTrack.maxHealth);
    }

    void OnDestroy()
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
}