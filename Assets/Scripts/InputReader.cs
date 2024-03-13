using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private PlayerCombat playerCombat;

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
        if (!playerCombat) return;

        if (inputContext.started)
        {
            playerCombat.Shoot();
        }
    }
}
