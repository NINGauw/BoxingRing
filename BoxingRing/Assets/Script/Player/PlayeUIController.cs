using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField]private Slider staminaSlider;
    public Slider healthSlider;
    [Header("Controller Reference")]
    [SerializeField]private PlayerController playerController;
    void Start()
    {
        if (staminaSlider == null)
        {
            enabled = false;
            return;
        }
        if (healthSlider == null)
        {
            return;
        }

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
            if (playerController == null)
            {
                enabled = false;
                return;
            }
        }

        if (staminaSlider != null && playerController != null) // Kiểm tra playerController trước khi đăng ký
        {
            playerController.OnStaminaChanged += UpdateStaminaBar;
        }
        if (healthSlider != null && playerController != null)
        {
            playerController.OnPlayerHealthChanged += UpdateHealthBar;
        }
    }

    void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnStaminaChanged -= UpdateStaminaBar;
            playerController.OnPlayerHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateStaminaBar(float currentStamina, float maxStamina)
    {
        if (maxStamina > 0)
        {
            staminaSlider.value = currentStamina / maxStamina;
        }
        else
        {
            staminaSlider.value = 0;
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