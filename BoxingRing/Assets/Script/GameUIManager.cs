using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    
    [SerializeField] private Button menuButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject menuPannel;
    [Header("WinPannel")]
    [SerializeField] private Button backToMenuButtonWin;
    [Header("LosePannel")]
    [SerializeField] private Button backToMenuButtonLose;

    void Start()
    {
        menuButton.onClick.AddListener(OnMenuButtonClicked);
        backToMenuButton.onClick.AddListener(OnBacktoMenuClicked);
        backToMenuButtonWin.onClick.AddListener(OnBacktoMenuClicked);
        backToMenuButtonLose.onClick.AddListener(OnBacktoMenuClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        menuPannel.SetActive(false);
    }

    private void OnMenuButtonClicked()
    {
        menuPannel.SetActive(true);
    }
    private void OnBacktoMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnExitButtonClicked()
    {
        menuPannel.SetActive(false);
    }
}
