using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Prepare to set up the instance
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is Null");
            }

            return _instance;
        }
    }

    // Connect to UI, SoundEffects, and SpawnManager
    private UIManager uiManagerScript;
    private SoundEffects soundEffectsScript;
    private SpawnManager spawnManagerScript;
    private NimbySpawner nimbySpawnerScript;

    // Stats
    public int currentStamina;
    public int score = 0;
    public int tokenCount = 0;
    public int nimbyCount = 0;

    // Gameplay values
    private int maxStamina = 300;
    private int depleteRateStamina = 1;
    private int foodValue = 60;
    private int foodPoints = 50;
    private int tokenPoints = 300;
    private int powerUpPoints = 350;
    private int nimbyPoints = 600;
    private int nimbyStaminaDecrease = 60;
    private int nimbyTokenSteal = 2;

    // Game active classes to pause game while menus are open
    public bool _isGameActive;

    public void GameActive(bool flag)
    {
        _isGameActive = flag;
    }

    public bool IsGameActive()
    {
        return _isGameActive;
    }

    // Game over classes to end game
    public bool _isGameOver;

    public void GameOver(bool flag)
    {
        _isGameOver = flag;

        // Reset stamina to 0 in case the player got hit at the end
        currentStamina = 0;
        uiManagerScript.UpdateStaminaUI(currentStamina);

        uiManagerScript.UpdateGameOverUI(nimbyCount, score);

        uiManagerScript.GameOverScreenOn();
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }
    
    // Set up the instance
    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        // Connect UI, SoundEffects, and SpawnManager
        uiManagerScript = GameObject.Find("Canvas").GetComponent<UIManager>();
        soundEffectsScript = GameObject.Find("Player").GetComponent<SoundEffects>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        nimbySpawnerScript = GameObject.Find("SpawnManager").GetComponent<NimbySpawner>();
    }

    // Start the game when the start button is clicked
    public void StartGame()
    {
        // Turn off the title menu
        uiManagerScript.TitleScreenOff();

        // Set game to active
        GameActive(true);

        // Set stamina and start depleting immediately
        SetMaxStamina(maxStamina);
        InvokeRepeating("DepleteStamina", 0.00001f, depleteRateStamina);

        // Start spawning
        spawnManagerScript.StartSpawn();
        nimbySpawnerScript.StartSpawn();

    }

    void Update()
    {
        if (IsGameActive())
        {
            // Check if the game is over
            CheckGameOver();
        }
    }

    void CheckGameOver()
    {
        // End the game if the player stamina is 0
        if (currentStamina <= 0)
        {
            GameActive(false);
            GameOver(true);
        }
    }

    void SetMaxStamina(int maxStamina)
    {
        currentStamina = maxStamina;
        uiManagerScript.SetMaxStaminaUI(maxStamina);
    }

    void DepleteStamina()
    {
        if (IsGameActive() && !IsGameOver())
        {
            currentStamina = currentStamina - depleteRateStamina;
            uiManagerScript.UpdateStaminaUI(currentStamina);
        }
    }

    public void EatFood()
    {
        soundEffectsScript.playOtherSound();

        if (currentStamina >= (maxStamina - foodValue))
        {
            currentStamina = maxStamina;
        }
        else if (currentStamina < (maxStamina - foodValue))
        {
            currentStamina += foodValue;
        }

        uiManagerScript.UpdateStaminaUI(currentStamina);

        score += foodPoints;
        uiManagerScript.UpdateScoreUI(score);

        Debug.Log("You eat some food.");
    }

    public void PickUpToken()
    {
        soundEffectsScript.playTokenSound();

        tokenCount++;
        uiManagerScript.UpdateTokenUI(tokenCount);

        score += tokenPoints;
        uiManagerScript.UpdateScoreUI(score);

        Debug.Log("You pick up a token.");
    }

    public void PowerUp()
    {
        soundEffectsScript.playOtherSound();

        score += powerUpPoints;
        uiManagerScript.UpdateScoreUI(score);
        
        Debug.Log("You power up!");
    }

    public void DestroyNimby()
    {
        soundEffectsScript.playDestroyNimbySound();

        nimbyCount++;
        score += nimbyPoints;
        uiManagerScript.UpdateScoreUI(score);
        
        Debug.Log("The NIMBY yielded to socially productive growth!");
    }

    public void AttackedByNimby()
    {
        soundEffectsScript.playNimbyAttackSound();

        currentStamina -= nimbyStaminaDecrease;
        uiManagerScript.UpdateStaminaUI(currentStamina);

        tokenCount -= nimbyTokenSteal;
        uiManagerScript.UpdateTokenUI(tokenCount);

        if (tokenCount >= 0)
        {
            Debug.Log("You were attacked by a NIMBY! They took some of your tokens!");
        }
        else if (tokenCount < 0)
        {
            Debug.Log("You were attacked by a NIMBY! You are now in financial debt!");
        }
    }
}
