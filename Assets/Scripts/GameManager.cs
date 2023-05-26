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
    private PostProcessingBehaviour postProcessingBehaviourScript;
    private SoundEffects soundEffectsScript;
    private PlayerController playerControllerScript;
    private SpawnManager spawnManagerScript;
    private NimbySpawner nimbySpawnerScript;

    // Stats
    public int currentStamina;
    public int score = 0;
    public int tokenCount = 0;
    public int nimbyCount = 0;

    // Gameplay values
    public int maxStamina = 300;
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
        postProcessingBehaviourScript.UpdateStaminaVignette(currentStamina);

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

    // Set up references to the other scripts
    void Start()
    {
        // Connect UI, SoundEffects, and SpawnManager
        uiManagerScript = GameObject.Find("Canvas").GetComponent<UIManager>();
        postProcessingBehaviourScript = GameObject.Find("Global Post Processing").GetComponent<PostProcessingBehaviour>();
        soundEffectsScript = GameObject.Find("Player").GetComponent<SoundEffects>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
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

    // If the game is active, check if it should actually be game over
    void Update()
    {
        if (IsGameActive())
        {
            // Check if the game is over
            CheckGameOver();
        }
    }

    // Check if the game is over
    void CheckGameOver()
    {
        // End the game if the player stamina is 0
        if (currentStamina <= 0)
        {
            GameActive(false);
            GameOver(true);
        }
    }
    
    // Assign the max stamina value to the player and update it in the UI
    void SetMaxStamina(int maxStamina)
    {
        currentStamina = maxStamina;
        uiManagerScript.SetMaxStaminaUI(maxStamina);
    }

    // Deplete player stamina at a specified rate as the game continues and update it in the UI
    void DepleteStamina()
    {
        if (IsGameActive() && !IsGameOver())
        {
            currentStamina = currentStamina - depleteRateStamina;
            uiManagerScript.UpdateStaminaUI(currentStamina);
            postProcessingBehaviourScript.UpdateStaminaVignette(currentStamina);
        }
    }

    // Recover player stamina and increase score when they eat food, update it in the UI and post processing, and play a sound
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
        postProcessingBehaviourScript.UpdateStaminaVignette(currentStamina);

        score += foodPoints;
        uiManagerScript.UpdateScoreUI(score);

        Debug.Log("You eat some food.");
    }

    // Increase the token count and score when the player picks up a token, and play a sound
    public void PickUpToken()
    {
        soundEffectsScript.playTokenSound();

        tokenCount++;
        uiManagerScript.UpdateTokenUI(tokenCount);

        score += tokenPoints;
        uiManagerScript.UpdateScoreUI(score);

        Debug.Log("You pick up a token.");
    }

    // Increase the score when the player picks up a power up and play a sound
    public void PowerUp()
    {
        soundEffectsScript.playOtherSound();

        score += powerUpPoints;
        uiManagerScript.UpdateScoreUI(score);
        
        Debug.Log("You power up!");
    }

    // Increase the score and nimby count when the player collides with a NIMBY while powered up, update it in the UI, and play a sound
    public void DestroyNimby()
    {
        soundEffectsScript.playDestroyNimbySound();

        nimbyCount++;
        score += nimbyPoints;
        uiManagerScript.UpdateScoreUI(score);
        
        Debug.Log("The NIMBY yielded to socially productive growth!");
    }

    // Decrease stamina and token count when the player is hit by a NIMBY while not powered up, update it in the UI and post processing, and play a sound
    public void AttackedByNimby()
    {
        soundEffectsScript.playNimbyAttackSound();

        currentStamina -= nimbyStaminaDecrease;
        uiManagerScript.UpdateStaminaUI(currentStamina);
        postProcessingBehaviourScript.UpdateStaminaVignette(currentStamina);

        postProcessingBehaviourScript.EnableChromaticAberration(playerControllerScript.slowedTime);

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
