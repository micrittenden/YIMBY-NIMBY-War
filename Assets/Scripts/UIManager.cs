using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text staminaText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text tokenText;

    public void UpdateStaminaUI(float currentStamina)
    {
        staminaText.text = "Stamina: " + currentStamina;
    }

    public void UpdateScoreUI(float score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateTokenUI(float tokenCount)
    {
        tokenText.text = "Tokens: " + tokenCount;
    }
}
