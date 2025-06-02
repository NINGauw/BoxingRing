using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonControl : MonoBehaviour
{
    public Button dodgeLeftButton;
    public Button dodgeRightButton;
    public PlayerController playerController;
    void Start()
    {
        dodgeLeftButton.onClick.AddListener(OnLeftButtonTapped);
        dodgeRightButton.onClick.AddListener(OnRightButtonTapped);
    }
    void OnLeftButtonTapped()
    {
        playerController.AttemptDodgeLeft();
    }
    public void OnRightButtonTapped()
    {
        playerController.AttemptDodgeRight();
    }
}
