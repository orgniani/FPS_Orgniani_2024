using UnityEngine;

public class UISplashScreen : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        levelManager.BackToMenu();
    }
}