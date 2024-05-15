using System.Collections;
using UnityEngine;

public class UISplashScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private float waitToSetActive = 2.4f;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SplashScreenShown"))
        {
            PlayerPrefs.SetInt("SplashScreenShown", 1);
            StartCoroutine(WaitToSetActiveMenu());
        }
        else
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator WaitToSetActiveMenu()
    {
        yield return new WaitForSeconds(waitToSetActive);

        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}