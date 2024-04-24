using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image gunSight;

    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image healthBar;

    [Header("Player References")]
    [SerializeField] private ShootController shooting;
    [SerializeField] private FirstPersonController player;
    [SerializeField] private HealthController playerHP;

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

    private void Update()
    {
        ChangeGunSightColor();
    }

    private void HandleHPText()
    {
        if (!HPText) return;
        HPText.text = playerHP.Health + "%";
    }

    private void HandleHealthBar()
    {
        healthBar.fillAmount = 1.0f * playerHP.Health / playerHP.MaxHealth;
    }

    private void ChangeGunSightColor()
    {
        if (!gunSight) return;

        Color originalColor = gunSight.color;

        Color newColor = shooting.isPointingAtEnemy ? new Color(1, 0, 0, originalColor.a) : new Color(1, 1, 1, originalColor.a);

        gunSight.color = newColor;
    }
}
