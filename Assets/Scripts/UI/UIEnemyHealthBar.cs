using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthController enemyHP;
    [SerializeField] private Image healthBar;

    [SerializeField] private GameObject clickToDragMessage;

    private void OnEnable()
    {
        enemyHP.onHurt += HandleHealthBar;
        enemyHP.onRevive += HandleHideInstructions;
    }

    private void OnDisable()
    {
        enemyHP.onDead -= HandleHealthBar;
        enemyHP.onRevive -= HandleHideInstructions;
    }

    private void HandleHealthBar()
    {
        //healthBar.fillAmount = 1.0f * enemyHP.Health / enemyHP.MaxHealth;
        clickToDragMessage.SetActive(true);
    }

    private void HandleHideInstructions()
    {
        //healthBar.fillAmount = 1.0f * enemyHP.Health / enemyHP.MaxHealth;
        clickToDragMessage.SetActive(false);
    }
}
