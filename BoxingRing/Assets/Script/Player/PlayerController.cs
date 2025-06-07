using UnityEngine;
using System;
using UnityEngine.EventSystems;
public class PlayerController : MonoBehaviour
{
    public LevelManager levelManager;
    // khoảng cách vuốt/tap tối thiểu để hợp lệ
    [SerializeField] private float minSwipeDistance = 50f;
    [SerializeField]private float maxTapDuration = 0.3f;
    [SerializeField]private float maxTapDistance = 30f;

    //Animation
    [SerializeField]private Animator playerAnimator;
    public EnemyController currentEnemy;

    //DelayParameter
    public float leftPunchImpactDelay = 0.5f;
    public float rightPunchImpactDelay = 0.5f;
    public float upperPunchImpactDelay = 0.5f;
    public float headPunchImpactDelay = 0.3f;

    //HP
    [Header("Player Health")]
    [SerializeField]private float maxPlayerHealth = 10f;
    private float currentPlayerHealth;
    public event Action<float, float> OnPlayerHealthChanged;

    //Dps Value
    [Header("Damage Values")]
    [SerializeField]private float leftRightPunchDamage = 1f;
    [SerializeField]private float upperPunchDamage = 1f;
    [SerializeField]private float headPunchDamage = 0.5f;

    // Vị trí đầu
    private Vector2 touchStartPos;
    private float touchStartTime;
    private bool isInputActive = false;

    // Stamina
    public float maxStamina = 100f;
    private float currentStamina;
    private float staminaRegenRate = 10f;    // stamina hồi mỗi giây
    public float staminaRegenDelay = 0.5f; // delay đến khi bắt đầu hồi stamina
    private float timeSinceLastStaminaUse = 0f; // đếm thời gian sau khi dùng stamina

    //Stamina cost
    private float leftRightPunchStaminaCost = 20f;
    private float upperPunchStaminaCost = 30f;
    private float headPunchStaminaCost = 15f;
    private float dodgeStaminaCost = 10f;

    //Eveny khi stamina change
    public event Action<float, float> OnStaminaChanged;

    //Animation const
    //Attack
    private const string TRIGGER_LEFT_PUNCH = "PunchLeft";
    private const string TRIGGER_RIGHT_PUNCH = "PunchRight";
    private const string TRIGGER_UPPER_PUNCH = "Upper";
    private const string PLAYER_IDLE_STATE_NAME = "Idle";
    private const string TRIGGER_HEAD_PUNCH = "HeadPunch";
    private const string TRIGGER_DODGE_LEFT = "DodgeLeft";
    private const string TRIGGER_DODGE_RIGHT = "DodgeRight";
    // Hitted
    public const string TRIGGER_PLAYER_HIT_BY_LEFT_RIGHT_PUNCH = "HitByRightLeftPunch";
    public const string TRIGGER_PLAYER_HIT_BY_UPPER_PUNCH = "HitByUpper";
    public const string TRIGGER_PLAYER_HIT_BY_HEAD_PUNCH = "HitByHeadPunch";
    void Start()
    {
        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<Animator>();
        }
        //Regen Stamina
        currentStamina = maxStamina;
        timeSinceLastStaminaUse = staminaRegenDelay;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        //Regen HP
        currentPlayerHealth = maxPlayerHealth;
        OnPlayerHealthChanged?.Invoke(currentPlayerHealth, maxPlayerHealth);
    }

    void Update()
    {
        // check ở Unity Editor
        HandleMouseInput();

        // Phone check
        HandleTouchInput();

        //Handle Stamina
        if (currentStamina < maxStamina)
        {
            timeSinceLastStaminaUse += Time.deltaTime;
            if (timeSinceLastStaminaUse >= staminaRegenDelay)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                isInputActive = false;
                return;
            }
            touchStartPos = Input.mousePosition;
            touchStartTime = Time.time;
            isInputActive = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isInputActive)
            {
                Vector2 touchEndPos = Input.mousePosition;
                float touchDuration = Time.time - touchStartTime;
                ProcessInput(touchStartPos, touchEndPos, touchDuration);
            }
            isInputActive = false;
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        isInputActive = false;
                    }
                    else
                    {
                        touchStartPos = touch.position;
                        touchStartTime = Time.time;
                        isInputActive = true;

                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isInputActive)
                    {
                        Vector2 touchEndPos = touch.position;
                        float touchDuration = Time.time - touchStartTime;
                        ProcessInput(touchStartPos, touchEndPos, touchDuration);
                    }
                    isInputActive = false;
                    break;
            }
        }
    }

    void ProcessInput(Vector2 startPos, Vector2 endPos, float duration)
    {
        AnimatorStateInfo currentAnimatorState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnimatorState.IsName(PLAYER_IDLE_STATE_NAME))
        {
            return;
        }

        float distanceMoved = Vector2.Distance(startPos, endPos);

        if (duration <= maxTapDuration && distanceMoved <= maxTapDistance)
        {
            HeadPunchAction();
            return;
        }

        if (distanceMoved >= minSwipeDistance)
        {
            ProcessSwipeAction(startPos, endPos);
            return;
        }
    }

    void HeadPunchAction()
    {
        if (currentStamina < headPunchStaminaCost)
        {
            return;
        }

        //Đấm thẳng
        currentStamina -= headPunchStaminaCost;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        timeSinceLastStaminaUse = 0f;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);

        playerAnimator.SetTrigger(TRIGGER_HEAD_PUNCH);

        if (currentEnemy != null)
        {
            currentEnemy.TakeHitOnHeadPunch(headPunchImpactDelay, headPunchDamage);
        }
    }

    void ProcessSwipeAction(Vector2 startPos, Vector2 endPos)
    {
        float deltaX = endPos.x - startPos.x;
        float deltaY = endPos.y - startPos.y;

        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) // Ngang
        {
            if (deltaX > 0) //Trái sang phải
            {
                if (playerAnimator != null)
                {
                    if (currentStamina < leftRightPunchStaminaCost)
                    {
                        Debug.Log("Không đủ Stamina, Cần: " + leftRightPunchStaminaCost + ", Hiện có: " + currentStamina);
                        return;
                    }
                    currentStamina -= leftRightPunchStaminaCost; //trừ stamina
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                    timeSinceLastStaminaUse = 0f; // Reset counter hồi stamina
                    OnStaminaChanged?.Invoke(currentStamina, maxStamina);
                    // Trigger Animation
                    playerAnimator.SetTrigger(TRIGGER_LEFT_PUNCH);
                    // Trigger Enemy Animation
                    if (currentEnemy != null)
                    {
                        currentEnemy.TakeHitOnLeftRightPunch(leftPunchImpactDelay, leftRightPunchDamage);
                    }
                }
            }
            else
            {
                if (playerAnimator != null)
                {
                    if (currentStamina < leftRightPunchStaminaCost)
                    {
                        Debug.Log("Không đủ Stamina, Cần: " + leftRightPunchStaminaCost + ", Hiện có: " + currentStamina);
                        return;
                    }
                    currentStamina -= leftRightPunchStaminaCost; //trừ stamina
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                    timeSinceLastStaminaUse = 0f; // Reset counter hồi stamina
                    OnStaminaChanged?.Invoke(currentStamina, maxStamina);
                    // Trigger Animation
                    playerAnimator.SetTrigger(TRIGGER_RIGHT_PUNCH);
                    // Trigger Enemy Animation
                    if (currentEnemy != null)
                    {
                        currentEnemy.TakeHitOnLeftRightPunch(rightPunchImpactDelay, leftRightPunchDamage);
                    }
                }
            }
        }
        else // Dọc
        {
            if (deltaY > 0) // Dưới lên trên
            {
                if (playerAnimator != null)
                {
                    if (currentStamina < upperPunchStaminaCost)
                    {
                        Debug.Log("Không đủ Stamina, Cần: " + upperPunchStaminaCost + ", Hiện có: " + currentStamina);
                        return;
                    }
                    currentStamina -= upperPunchStaminaCost; //trừ stamina
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                    timeSinceLastStaminaUse = 0f; // Reset counter hồi stamina
                    OnStaminaChanged?.Invoke(currentStamina, maxStamina);
                    // Trigger Animation
                    playerAnimator.SetTrigger(TRIGGER_UPPER_PUNCH);
                    // Trigger Enemy Animation
                    if (currentEnemy != null)
                    {
                        currentEnemy.TakeHitOnUpperPunch(upperPunchImpactDelay, upperPunchDamage);
                    }
                }
            }
            else
            {
                Debug.Log("kéo xuống");
                //Đang tìm animation đòn chẻ xuống
            }
        }
    }
    public void AttemptDodgeLeft()
    {
        ProcessDodgeAction(true);
    }

    public void AttemptDodgeRight()
    {
        ProcessDodgeAction(false);
    }

    private void ProcessDodgeAction(bool isLeftDodge)
    {
        AnimatorStateInfo currentAnimatorState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnimatorState.IsName(PLAYER_IDLE_STATE_NAME))
        {
            return;
        }

        if (currentStamina < dodgeStaminaCost)
        {
            return;
        }

        currentStamina -= dodgeStaminaCost;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        timeSinceLastStaminaUse = 0f;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);

        if (isLeftDodge)
        {
            playerAnimator.SetTrigger(TRIGGER_DODGE_LEFT);
        }
        else
        {
            playerAnimator.SetTrigger(TRIGGER_DODGE_RIGHT);
        }
    }
    public void TakeDamage(float amount, string hitAnimationTriggerName)
    {
        if (currentPlayerHealth <= 0) return;
        currentPlayerHealth -= amount;
        currentPlayerHealth = Mathf.Clamp(currentPlayerHealth, 0f, maxPlayerHealth);
        OnPlayerHealthChanged?.Invoke(currentPlayerHealth, maxPlayerHealth);

        if (currentPlayerHealth <= 0)
        {
            PlayerDie();
        }
        if (!IsDead() && playerAnimator != null && !string.IsNullOrEmpty(hitAnimationTriggerName))
        {
            playerAnimator.SetTrigger(hitAnimationTriggerName);
        }
    }
    public bool IsDead()
    {
        return currentPlayerHealth <= 0;
    }

    void PlayerDie()
    {
        playerAnimator.SetTrigger("KnockOut");
        if (OneVsOneManager.Instance != null)
        {
            OneVsOneManager.Instance.LoseRound();
        }
        if (OneVsManyManager.Instance != null)
        {
            OneVsManyManager.Instance.LoseRound();
        }
        if (levelManager != null)
        {
            levelManager.LostRound();   
        }

    }
}