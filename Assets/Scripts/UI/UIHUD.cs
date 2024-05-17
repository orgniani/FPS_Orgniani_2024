using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHUD : MonoBehaviour
{
    [Header("UI References")]
    [Header("Gun")]
    [SerializeField] private Image gunSight;
    [SerializeField] private TextMeshProUGUI ammoAmountText;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image healthBar;

    [Header("Bloody Screen")]
    [SerializeField] private Image bloodScreen;
    [SerializeField] private float bloodScreenFadeDuration = 1.0f;
    [SerializeField] private AnimationCurve bloodScreenFadeCurve;

    [Header("Forest")]
    [SerializeField] private TextMeshProUGUI forestHPText;
    [SerializeField] private Image forestHealthBar;

    [Header("Weapon Icons")]
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
        playerHP.onHPChange += HandleHPText;
        playerHP.onHPChange += HandleHealthBar;
        playerHP.onHurt += HandleBloodOnScreen;

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
        playerHP.onHPChange -= HandleHPText;
        playerHP.onHPChange -= HandleHealthBar;
        playerHP.onHurt -= HandleBloodOnScreen;

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

    private void HandleBloodOnScreen()
    {
        if (!bloodScreen) return;

        bloodScreen.gameObject.SetActive(true);
        StartCoroutine(FadeOutBloodScreen());
    }

    private IEnumerator FadeOutBloodScreen()
    {
        float elapsedTime = 0;
        Color originalColor = bloodScreen.color;

        while (elapsedTime < bloodScreenFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / bloodScreenFadeDuration;
            float curveValue = bloodScreenFadeCurve.Evaluate(t);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - curveValue);
            bloodScreen.color = newColor;

            yield return null;
        }

        bloodScreen.gameObject.SetActive(false);
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
