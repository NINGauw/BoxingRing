using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // khoảng cách vuốt tối  thiểu để hợp lệ
    public float minSwipeDistance = 50f;
    public Animator playerAnimator;
    public EnemyController currentEnemy;
    public float leftPunchImpactDelay = 0.5f;
    public float rightPunchImpactDelay = 0.5f;
    public float upperPunchImpactDelay = 0.5f;
    // Vị trí đầu
    private Vector2 touchStartPos;
    // Vị trí cuối
    private Vector2 touchEndPos;
    private bool isSwiping = false;
    //Animation const
    private const string TRIGGER_LEFT_PUNCH = "PunchLeft";
    private const string TRIGGER_RIGHT_PUNCH = "PunchRight";
    private const string TRIGGER_UPPER_PUNCH = "Upper";
    private const string PLAYER_IDLE_STATE_NAME = "Idle";


    void Start()
    {
        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<Animator>();
        }
    }
    void Update()
    {
        // check ở Unity Editor
        HandleMouseInput();

        // Phone check
        HandleTouchInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            if (isSwiping)
            {
                touchEndPos = Input.mousePosition;
                ProcessSwipe();
                isSwiping = false;
            }
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
                    touchStartPos = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isSwiping)
                    {
                        touchEndPos = touch.position;
                        ProcessSwipe();
                        isSwiping = false;
                    }
                    break;
            }
        }
    }

    void ProcessSwipe()
    {
        AnimatorStateInfo currentAnimatorState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        float deltaX = touchEndPos.x - touchStartPos.x;
        float deltaY = touchEndPos.y - touchStartPos.y;

        float swipeDistance = Vector2.Distance(touchStartPos, touchEndPos);
        if (!currentAnimatorState.IsName(PLAYER_IDLE_STATE_NAME))
        {
            return;
        }
        if (swipeDistance < minSwipeDistance)
        {
            return;
        }

        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) // Ngang
        {
            if (deltaX > 0) //Trái sang phải
            {
                if (playerAnimator != null)
                {
                    // Trigger Animation
                    playerAnimator.SetTrigger(TRIGGER_LEFT_PUNCH);
                    // Trigger Enemy Animation
                    if (currentEnemy != null)
                    {
                        currentEnemy.TakeHitOnLeftRightPunch(leftPunchImpactDelay, 1);
                    }
                }
            }
            else //Phải sang trái
            {
                if (playerAnimator != null)
                {
                    // Trigger Animation
                    playerAnimator.SetTrigger(TRIGGER_RIGHT_PUNCH);
                    // Trigger Enemy Animation
                    if (currentEnemy != null)
                    {
                        currentEnemy.TakeHitOnLeftRightPunch(rightPunchImpactDelay, 1);
                    }
                }
            }
        }
        else
        {
            if (deltaY > 0) // Dưới lên trên
            {
                if (playerAnimator != null)
                {
                    // Trigger Animation
                    playerAnimator.SetTrigger(TRIGGER_UPPER_PUNCH);
                    // Trigger Enemy Animation
                    if (currentEnemy != null)
                    {
                        currentEnemy.TakeHitOnUpperPunch(upperPunchImpactDelay, 1);
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
}