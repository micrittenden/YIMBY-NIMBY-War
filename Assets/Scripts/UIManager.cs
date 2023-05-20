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
    // The UI buttons have OnClick() components added via the Inspector to change between screens
    // The stats are updated via classes in the GameManager script

    // Stats
    public Slider staminaSlider;
    public Gradient staminaGradient;
    public Image staminaFill;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text tokenText;
    [SerializeField] TMP_Text gameOverText;

    // Screens
    public GameObject TitleScreen;
    public GameObject SettingsScreen;
    public GameObject MainScreen;
    public GameObject MenuScreen;
    public GameObject GameOverScreen;

    public void StartButton()
    {
        GameManager.Instance.StartGame();
        MainScreenOn();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void SetMaxStaminaUI(int maxStamina)
    {
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;

        staminaFill.color = staminaGradient.Evaluate(1.0f);
    }

    public void UpdateStaminaUI(int currentStamina)
    {
        staminaSlider.value = currentStamina;

        staminaFill.color = staminaGradient.Evaluate(staminaSlider.normalizedValue);

    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateTokenUI(int tokenCount)
    {
        tokenText.text = "Tokens: " + tokenCount;
    }

    public void TitleScreenOn()
    {
        TitleScreen.SetActive(true);
    }

     public void TitleScreenOff()
    {
        TitleScreen.SetActive(false);
    }

    public void SettingsScreenOn()
    {
        TitleScreenOff();
        SettingsScreen.SetActive(true);
    }

    public void SettingsScreenOff()
    {
        SettingsScreen.SetActive(false);    
        TitleScreenOn();
    }

    public void MainScreenOn()
    {
        MainScreen.SetActive(true);
    }

    public void MainScreenOff()
    {
        MainScreen.SetActive(false);
    }

    public void MenuScreenOn()
    {
        MenuScreen.SetActive(true);
        GameManager.Instance.GameActive(false);
    }

    public void MenuScreenOff()
    {
        MenuScreen.SetActive(false);
        GameManager.Instance.GameActive(true);
    }

    public void MenuScreenQuit()
    {
        MenuScreen.SetActive(false);
        GameManager.Instance.GameOver(true);
    }

    public void UpdateGameOverUI(int nimbyCount, int score)
    {
        gameOverText.text = "You defeated " + nimbyCount + " NIMBYs, but in the end they were too powerful. You retire from your quest to make cities more sustainable, livable, and affordable. You finished with a score of " + score + ". Not bad, but you could do better!";
    }

    public void GameOverScreenOn()
    {
        MainScreenOff();
        GameOverScreen.SetActive(true);
    }
}