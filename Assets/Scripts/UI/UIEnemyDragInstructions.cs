using UnityEngine;
public class UIEnemyDragInstructions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthController enemyHP;
    [SerializeField] private GameObject clickToDragMessage;

    private void OnEnable()
    {
        enemyHP.onHPChange += HandleShowInstructions;
        enemyHP.onRevive += HandleHideInstructions;
    }

    private void OnDisable()
    {
        enemyHP.onDead -= HandleShowInstructions;
        enemyHP.onRevive -= HandleHideInstructions;
    }

    private void HandleShowInstructions()
    {
        clickToDragMessage.SetActive(true);
    }

    private void HandleHideInstructions()
    {
        clickToDragMessage.SetActive(false);
    }
}
