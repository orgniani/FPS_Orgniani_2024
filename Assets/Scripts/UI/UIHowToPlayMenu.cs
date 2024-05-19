using UnityEngine;
using TMPro;

public class UIHowToPlayMenu : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private string[] instructions = new string[]
    {
        "1. Combat the Goblins!\n\nThe forest (and you) are under attack by goblins! Use your tranquilizer gun to make them fall asleep.",
        "2. Secure the Goblins!\n\nOnce a goblin is asleep, disarm and drag it to the nearest cabin to trap it inside!",
        "3. Act Quickly!\n\nThe tranquilizer effect is short-lived, so move fast! Trap the goblin before it wakes up again.",
        "4. Make each shot count! \n\n You have limited tranquilizer darts. Look for crates to replenish your ammunition and first aid kits to restore your health.",
        "5. Extinguish Fires!\n\nUse the fire extinguisher to put out any fires before the forest is destroyed.",
        "6. Win!\n\nTrap all the goblins before you or the forest are destroyed to win!"
    };

    private int currentInstructionIndex = 0;

    private void OnEnable()
    {
        currentInstructionIndex = 0;
        UpdateInstructionText();
    }

    public void OnNextPage()
    {
        currentInstructionIndex = Mathf.Min(currentInstructionIndex + 1, instructions.Length - 1);
        UpdateInstructionText();
    }


    public void OnBackPage()
    {
        currentInstructionIndex = Mathf.Max(currentInstructionIndex - 1, 0);
        UpdateInstructionText();
    }

    private void UpdateInstructionText()
    {
        instructionText.text = instructions[currentInstructionIndex];
    }
}
