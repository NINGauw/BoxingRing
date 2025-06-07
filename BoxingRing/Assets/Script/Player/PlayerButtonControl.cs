using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonControl : MonoBehaviour
{
    [SerializeField]private Button dodgeLeftButton;
    [SerializeField]private Button dodgeRightButton;
    [SerializeField]private PlayerController playerController;
    [SerializeField]private DodgeSystem playerDodge;
    void Start()
    {
        dodgeLeftButton.onClick.AddListener(OnLeftButtonTapped);
        dodgeRightButton.onClick.AddListener(OnRightButtonTapped);
    }
    void OnLeftButtonTapped()
    {
        playerController.AttemptDodgeLeft();
        playerDodge.StartDodgeLeft();
    }
    public void OnRightButtonTapped()
    {
        playerController.AttemptDodgeRight();
        playerDodge.StartDodgeLeft();
    }
}
