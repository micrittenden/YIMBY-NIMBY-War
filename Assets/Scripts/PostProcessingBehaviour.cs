using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make sure the Post Processing package has been installed to the project.

public class PostProcessingBehaviour : MonoBehaviour
{
    private float staminaWarningLevel = 0.2f;

    public void UpdateStaminaVignette(int currentStamina)
    {
        // Add effect which makes the screen more red from the edges inward as stamina decreases
        if (currentStamina <= (GameManager.Instance.maxStamina * staminaWarningLevel))
        {
            Debug.Log("Your stamina is dangerously low!");
        }
        else
        {
            // nothing happens. Alternatively, we can do this without a condition if we set the value logarithmically
        }
    }
}
