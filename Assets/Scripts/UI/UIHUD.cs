using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image gunSight;
    [SerializeField] private TextMeshProUGUI ammoAmountText;

    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image healthBar;

    [SerializeField] private TextMeshProUGUI forestHPText;
    [SerializeField] private Image forestHealthBar;

    [SerializeField] private GameObject gunIcon;
    [SerializeField] private GameObject extinguisherIcon;
    [SerializeField] private GameObject noneIcon;

    [Header("Player References")]
    [SerializeField] private GunController shooting;
    [SerializeField] private FirstPersonController player;
    [SerializeField] private HealthController playerHP;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AttackSwapController attackSwapController;

    private void OnEnable()
    {
        playerHP.onHurt += HandleHPText;
        playerHP.onHurt += HandleHealthBar;

        gameManager.onNewDeadTree += HandleForestHPText;
        gameManager.onNewDeadTree += HandleForestHealthBar;

        shooting.onAmmoChange += HandleAmmoAmountText;

        PickUpItems.onPickUp += HandleAddWeaponIcon;

        HandleAmmoAmountText();
        HandleHealthBar();
        HandleHPText();
    }

    private void OnDisable()
    {
        playerHP.onHurt -= HandleHPText;
        playerHP.onHurt -= HandleHealthBar;

        gameManager.onNewDeadTree -= HandleForestHPText;
        gameManager.onNewDeadTree -= HandleForestHealthBar;

        PickUpItems.onPickUp -= HandleAddWeaponIcon;
    }

    private void Update()
    {
        ChangeGunSightColor();
    }

    private void HandleHPText()
    {
        if (!HPText) return;
        HPText.text = "HP: " + playerHP.Health + "%";
    }

    private void HandleHealthBar()
    {
        if (!healthBar) return;
        healthBar.fillAmount = 1.0f * playerHP.Health / playerHP.MaxHealth;
    }

    private void HandleForestHPText()
    {
        if (!forestHPText) return;

        float percentage = 1.0f * gameManager.flammables.Count / gameManager.flammablesTotal * 100;
        int roundedPercentage = Mathf.RoundToInt(percentage);
        forestHPText.text = "FOREST: " + roundedPercentage + "%";
    }

    private void HandleForestHealthBar()
    {
        if (!forestHealthBar) return;
        forestHealthBar.fillAmount = 1.0f * gameManager.flammables.Count / gameManager.flammablesTotal;
    }

    private void ChangeGunSightColor()
    {
        if (!gunSight) return;

        Color originalColor = gunSight.color;

        Color newColor = shooting.isPointingAtEnemy ? new Color(1, 0, 0, originalColor.a) : new Color(1, 1, 1, originalColor.a);

        gunSight.color = newColor;
    }

    private void HandleAmmoAmountText()
    {
        if (!ammoAmountText) return;

        ammoAmountText.text = "x" + shooting.AmmoAmount;
    }

    private void HandleAddWeaponIcon()
    {
        if(attackSwapController.AquiredGun)
        {
            gunIcon.SetActive(true);
            gunSight.gameObject.SetActive(true);
        }

        if (attackSwapController.AquiredExtinguisher)
        {
            extinguisherIcon.SetActive(true);
        }

        noneIcon.SetActive(true);
    }
}
