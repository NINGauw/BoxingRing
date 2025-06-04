using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private GameObject gameModePannel;
    [SerializeField] private Button OneVsOneButton;
    [SerializeField] private GameObject settingPannel;
    [SerializeField] private Button exitGameModeButton;
    [SerializeField] private Button exitSettingButton;
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingButton.onClick.AddListener(SettingButtonOnClicked);
        exitGameModeButton.onClick.AddListener(ExitButtonOnClicked);
        exitSettingButton.onClick.AddListener(ExitButtonOnClicked);
        OneVsOneButton.onClick.AddListener(OneVsOneClicked);
        gameModePannel.SetActive(false);
        settingPannel.SetActive(false);
    }

    private void OnPlayButtonClicked()
    {
        gameModePannel.SetActive(true);
    }
    private void SettingButtonOnClicked()
    {
        settingPannel.SetActive(true);
    }
    private void ExitButtonOnClicked()
    {
        if (gameModePannel != null)
            gameModePannel.SetActive(false);
        if (settingPannel != null)
            settingPannel.SetActive(false);
    }
    private void OneVsOneClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
}
