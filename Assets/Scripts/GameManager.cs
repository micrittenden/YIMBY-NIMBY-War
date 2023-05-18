using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Set up the instance
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

    // Connect to UI
    private UIManager uiManagerScript;

    // Stats
    public float currentStamina;
    public int score = 0;
    public int tokenCount = 0;

    // Gameplay values
    private float maxStamina = 100.0f;
    private float depleteRateStamina = 1.0f;
    private float foodValue = 30.0f;
    private int foodPoints = 5;
    private int tokenPoints = 10;
    private int powerUpPoints = 20;
    private int nimbyPoints = 40;
    private int nimbyStaminaDecrease = 15;
    private int nimbyTokenSteal = 2;

    void Start()
    {
        // Set stamina and start depleting immediately
        currentStamina = maxStamina;
        InvokeRepeating("DepleteStamina", 0.00001f, depleteRateStamina);
        
        // Connect UI
        uiManagerScript = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void Update()
    {       
        // Check if the game is over
        CheckGameOver();
    }

    void CheckGameOver()
    {
        // End the game if the player stamina is 0
        if (currentStamina <= 0)
        {
            GameOver(true);

            CancelInvoke("DepleteStamina");

             // Reset in case NIMBYs hit the player at the end
            currentStamina = 0;
            uiManagerScript.UpdateStaminaUI(currentStamina);

            Debug.Log("The NIMBYs were too powerful. You retire from your quest to make cities more sustainable, livable, and affordable. You finished with a score of " + score + ". Not bad, but you could do better!");
        }
    }

    void DepleteStamina()
    {
        currentStamina = currentStamina - depleteRateStamina;
        uiManagerScript.UpdateStaminaUI(currentStamina);
    }

    public void EatFood()
    {
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
        tokenCount++;
        uiManagerScript.UpdateTokenUI(tokenCount);

        score += tokenPoints;
        uiManagerScript.UpdateScoreUI(score);

        Debug.Log("You pick up a token.");
    }

    public void PowerUp()
    {
        score += powerUpPoints;
        uiManagerScript.UpdateScoreUI(score);
        
        Debug.Log("You power up!");
    }

    public void DestroyNimby()
    {
        score += nimbyPoints;
        uiManagerScript.UpdateScoreUI(score);
        
        Debug.Log("The NIMBY yielded to socially productive growth!");
    }

    public void AttackedByNimby()
    {
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

    // Game over classes
    public bool _isGameOver;

    private void Awake()
    {
        _instance = this;
    }

    public void GameOver(bool flag)
    {
        _isGameOver = flag;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }
}
