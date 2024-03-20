using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarText : MonoBehaviour
{
    [SerializeField] private HealthController playerHP;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image healthBar;

    private void OnEnable()
    {
        playerHP.onHurt += HandleHPText;
        playerHP.onHurt += HandleHealthBar;
    }

    private void OnDisable()
    {
        playerHP.onHurt -= HandleHPText;
        playerHP.onHurt -= HandleHealthBar;
    }

    private void HandleHPText()
    {
        if (!HPText) return;
        HPText.text = playerHP.getHealth() + "%";
    }

    private void HandleHealthBar()
    {
        healthBar.fillAmount =  1.0f * playerHP.getHealth() / playerHP.getMaxHealth();
    }
}
