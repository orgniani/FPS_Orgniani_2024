using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthController enemyHP;
    [SerializeField] private Image healthBar;

    private void OnEnable()
    {
        enemyHP.onHurt += HandleHealthBar;
    }

    private void OnDisable()
    {
        enemyHP.onHurt -= HandleHealthBar;
    }

    private void HandleHealthBar()
    {
        healthBar.fillAmount = 1.0f * enemyHP.Health / enemyHP.MaxHealth;
    }
}
