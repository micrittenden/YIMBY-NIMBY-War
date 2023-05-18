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

    public void GameOverScreenOn()
    {
        MainScreenOff();
        GameOverScreen.SetActive(true);
    }
}