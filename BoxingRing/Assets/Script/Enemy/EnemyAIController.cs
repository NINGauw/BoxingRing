using UnityEngine;
using System.Collections; // Cần cho Coroutines

public class EnemyAIController : MonoBehaviour
{
    public Animator enemyAnimator;
    public PlayerController playerTarget;
    [Header("Attack Timing")]
    [SerializeField]private float minAttackDelay = 3.0f; // Time tối thiểu giữa các đợt tấn công
    [SerializeField]private float maxAttackDelay = 6.0f; // Time tối đa giữa các đợt tấn công
    private float attackTimer;

    [Header("Enemy Attack Stats")]
    [SerializeField]private float leftPunchDamage = 1f;
    [SerializeField]private float rightPunchDamage = 1f;
    [SerializeField]private float upperPunchDamage = 1f;
    [SerializeField]private float straightPunchDamage = 0.5f;

    //Delay Para
    public float leftPunchImpactDelay = 0.5f;
    public float rightPunchImpactDelay = 0.5f;
    public float upperPunchImpactDelay = 0.5f;
    public float straightPunchImpactDelay = 0.3f;

    // Tên các state và trigger trong EnemyAnimatorController
    private const string ENEMY_IDLE_STATE_NAME = "Idle";
    private const string ENEMY_TRIGGER_PUNCH_LEFT = "PunchLeft";
    private const string ENEMY_TRIGGER_PUNCH_RIGHT = "PunchRight";
    private const string ENEMY_TRIGGER_UPPER_PUNCH = "Upper";
    private const string ENEMY_TRIGGER_HEAD_PUNCH = "HeadPunch";

    private bool isAttacking = false;

    void Start()
    {
        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponent<Animator>();
        }
        if (enemyAnimator == null)
        {
            enabled = false;
            return;
        }

        if (playerTarget == null)
        {
            playerTarget = FindObjectOfType<PlayerController>();
        }
        if (playerTarget == null)
        {
            enabled = false;
            return;
        }
        ResetAttackTimer();
    }

    void Update()
    {
        if (playerTarget == null /*|| playerTarget.IsDead()*/) 
        {   
            return;
        }

        AnimatorStateInfo currentAnimatorState = enemyAnimator.GetCurrentAnimatorStateInfo(0);
        isAttacking = !currentAnimatorState.IsName(ENEMY_IDLE_STATE_NAME);

        if (!isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                ChooseAndPerformAttack();
                ResetAttackTimer();
            }
        }
    }

    void ResetAttackTimer()
    {
        attackTimer = Random.Range(minAttackDelay, maxAttackDelay);
    }

    void ChooseAndPerformAttack()
    {
        if (playerTarget == null || !playerTarget.gameObject.activeInHierarchy /*playerTarget.IsDead()*/) return;
        AnimatorStateInfo currentAnimatorState = enemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnimatorState.IsName(ENEMY_IDLE_STATE_NAME)) {
            return;
        }


        int numberOfAttacks = 4;
        int attackChoice = Random.Range(0, numberOfAttacks);

        string triggerToUse = "";
        float impactDelay = 0f;
        float damage = 0f;
        string playerHitReactionTrigger = "";

        switch (attackChoice)
        {
            case 0: //Left Punch
                triggerToUse = ENEMY_TRIGGER_PUNCH_LEFT;
                impactDelay = leftPunchImpactDelay;
                damage = leftPunchDamage;
                playerHitReactionTrigger = PlayerController.TRIGGER_PLAYER_HIT_BY_LEFT_RIGHT_PUNCH;
                break;
            case 1: //Right Punch
                triggerToUse = ENEMY_TRIGGER_PUNCH_RIGHT;
                impactDelay = rightPunchImpactDelay;
                damage = rightPunchDamage;
                playerHitReactionTrigger = PlayerController.TRIGGER_PLAYER_HIT_BY_LEFT_RIGHT_PUNCH;
                break;
            case 2: // Upper Punch
                triggerToUse = ENEMY_TRIGGER_UPPER_PUNCH;
                impactDelay = upperPunchImpactDelay;
                damage = upperPunchDamage;
                playerHitReactionTrigger = PlayerController.TRIGGER_PLAYER_HIT_BY_UPPER_PUNCH;
                break;
            case 3: // Head Punch
                triggerToUse = ENEMY_TRIGGER_HEAD_PUNCH;
                impactDelay = straightPunchImpactDelay;
                damage = straightPunchDamage;
                playerHitReactionTrigger = PlayerController.TRIGGER_PLAYER_HIT_BY_HEAD_PUNCH;
                break;
        }

        if (!string.IsNullOrEmpty(triggerToUse))
        {
            enemyAnimator.SetTrigger(triggerToUse);
            StartCoroutine(DealDamageToPlayerAfterDelay(impactDelay, damage, triggerToUse, playerHitReactionTrigger));
        }
    }

    IEnumerator DealDamageToPlayerAfterDelay(float delay, float damageAmount, string attackNameForDebug, string playerHitTrigger)
    {
        yield return new WaitForSeconds(delay);

        if (playerTarget != null && playerTarget.gameObject.activeInHierarchy && !playerTarget.IsDead())
        {
            Debug.Log($"Enemy's {attackNameForDebug} connects! Dealing {damageAmount} damage to Player.");
            playerTarget.TakeDamage(damageAmount, playerHitTrigger);
        }
    }
}