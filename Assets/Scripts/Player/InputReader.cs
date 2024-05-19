using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private GunController gun;
    [SerializeField] private FireExtinguisherController fireExtinguisher;
    [SerializeField] private HandController handController;
    [SerializeField] private AttackSwapController attackSwapController;
    [SerializeField] private MenuManager menuController;

    private void Start()
    {
        if(!playerController)
        {
            Debug.LogError($"{name}: FirstPersonController is null!");
            Debug.LogError($"Disabling component");
            gameObject.SetActive(false);
        }

        if(!gun)
        {
            Debug.LogError($"{name}: CharacterShooting is null!");
            Debug.LogError($"Disabling component");
            gameObject.SetActive(false);
        }

        if (!fireExtinguisher)
        {
            Debug.LogError($"{name}: FireExtinguisher is null!");
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
            gun.Shoot();
            fireExtinguisher.Spray(true);
            handController.Drag(true);
        }

        else if (inputContext.canceled)
        {
            fireExtinguisher.Spray(false);
            handController.Drag(false);
        }
    }

    public void PauseGame(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            menuController.PauseAndUnpauseGame();
        }
    }

    public void ChangeToGun(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            attackSwapController.SwapToGun();
        }
    }

    public void ChangeToFireExtinguisher(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            attackSwapController.SwapToFireExtinguisher();
        }
    }

    public void ChangeToHands(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            attackSwapController.SwapToHands();
        }
    }

    public void TrapGoblin(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            handController.HandleTrapGoblin();
        }
    }
}
