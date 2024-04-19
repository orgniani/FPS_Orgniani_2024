using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private ShootController characterShooting;
    [SerializeField] private MenuInputReader menuController;

    private void Start()
    {
        if(!playerController)
        {
            Debug.LogError($"{name}: FirstPersonController is null!");
            Debug.LogError($"Disabling component");
            gameObject.SetActive(false);
        }

        else if(!characterShooting )
        {
            Debug.LogError($"{name}: CharacterShooting is null!");
            Debug.LogError($"Disabling component");
            gameObject.SetActive(false);
        }
    }

    public void SetMovementValue(InputAction.CallbackContext inputContext)
    {
        Vector2 inputValue = inputContext.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputValue.x, 0f, inputValue.y);
        playerController.SetDirection(direction);
    }

    public void Jump(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            playerController.Jump();
        }
    }

    public void SetLookValues(InputAction.CallbackContext inputContext)
    {
        Vector2 inputValue = inputContext.ReadValue<Vector2>();
        playerController.Look(inputValue);
    }

    public void Sprint(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            playerController.Sprint(true);
        }

        else if (inputContext.canceled)
        {
            playerController.Sprint(false);
        }
    }

    public void Shoot(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            characterShooting.Shoot();
        }
    }

    public void PauseGame(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            menuController.PauseAndUnpauseGame();
        }
    }
}
