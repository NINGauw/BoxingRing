using UnityEngine;
using System.Collections;
using System;

public class EnemyController : MonoBehaviour
{
    public Animator enemyAnimator;
    public LevelManager levelManager;
    public float maxHealth = 10f;
    private float currentHealth;
    public event Action<float, float> OnHealthChanged;

    // Animation Const
    private const string TRIGGER_HIT_ON_LEFT_RIGHT = "HitByRightLeftPunch";
    private const string TRIGGER_HIT_ON_UPPER = "HitByUpper";
    private const string TRIGGER_HIT_ON_HEAD = "HitByHeadPunch";
    private const string KNOCK_OUT = "KnockOut";

    void Start()
    {
        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponent<Animator>();
        }
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetUpEnemyStats(float _maxHealth)
    {
        maxHealth = _maxHealth;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeHitOnLeftRightPunch(float delay, float damageAmmount)
    {
        if (enemyAnimator != null)
        {
            StartCoroutine(PlayHitAnimationDelayed(TRIGGER_HIT_ON_LEFT_RIGHT, delay, damageAmmount));
        }
    }

    public void TakeHitOnUpperPunch(float delay, float damageAmmount)
    {
        if (enemyAnimator != null)
        {
            StartCoroutine(PlayHitAnimationDelayed(TRIGGER_HIT_ON_UPPER, delay, damageAmmount));
        }
    }
    public void TakeHitOnHeadPunch(float delay, float damageAmmount)
    {
        if (enemyAnimator != null)
        {
            StartCoroutine(PlayHitAnimationDelayed(TRIGGER_HIT_ON_HEAD, delay, damageAmmount));
        }
    }

    private IEnumerator PlayHitAnimationDelayed(string hitTriggerName, float delay, float damageAmount)
    {
        yield return new WaitForSeconds(delay);
        ApplyDamage(damageAmount);
        if (currentHealth > 0)
        {
            enemyAnimator.SetTrigger(hitTriggerName);
        }
    }
    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        enemyAnimator.SetTrigger(KNOCK_OUT);
        if (OneVsOneManager.Instance != null)
        {
            OneVsOneManager.Instance.WinRound();
        }
        if (OneVsManyManager.Instance != null)
        {
            OneVsManyManager.Instance.OnEnemyDefeated();
        }
        if (levelManager != null)
        {
            levelManager.OnEnemyDefeated(this);
        }
        Invoke("DestroyEnemyDelay", 2f);
        
    }
    private void DestroyEnemyDelay()
    {
        Destroy(gameObject);
    }
}
