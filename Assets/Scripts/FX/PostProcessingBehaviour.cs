using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Make sure the Post Processing package has been installed to the project.

public class PostProcessingBehaviour : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private ChromaticAberration chromaticAbberation;
    private float staminaWarningLevel = 0.2f;
    
    void Start()
    {        
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<FilmGrain>(out filmGrain);
        volume.profile.TryGet<ChromaticAberration>(out chromaticAbberation);
    }

    public void UpdateStaminaVignette(int currentStamina)
    {
        // Turn on or off the vignette depending on current stamina
        if (currentStamina <= (GameManager.Instance.maxStamina * staminaWarningLevel))
        {
            Debug.Log("Your stamina is dangerously low!");
            vignette.active = true;
            filmGrain.active = true;
        }
        else
        {
            vignette.active = false;
            filmGrain.active = false;
        }
    }

    public void EnableChromaticAberration(float duration = 0)
    {
        // Turn on chromatic aberration
        chromaticAbberation.active = true;

        // If duration is not null
        if (duration != 0)
        {
            StartCoroutine(ChromaticAberrationCoroutine(duration));
        }
    }

    public void DisableChromaticAberration()
    {
        // Turn off chromatic aberration
        chromaticAbberation.active = false;
    }

    public IEnumerator ChromaticAberrationCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        DisableChromaticAberration();
    }
}
