using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private Image gunSight;

    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image healthBar;

    [SerializeField] private CharacterShooting shooting;
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
        HPText.text = playerHP.getHealth() + "%";
    }

    private void HandleHealthBar()
    {
        healthBar.fillAmount = 1.0f * playerHP.getHealth() / playerHP.getMaxHealth();
    }

    private void ChangeGunSightColor()
    {
        if (!gunSight) return;

        Color originalColor = gunSight.color;

        Color newColor = shooting.isPointingAtEnemy ? new Color(1, 0, 0, originalColor.a) : new Color(1, 1, 1, originalColor.a);

        gunSight.color = newColor;
    }
}
