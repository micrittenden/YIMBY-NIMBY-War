using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;

    private Animator playerAnim;
    private float idleAnim = 0f;
    private float walkAnim = 0.3f;
    private float runAnim = 0.6f;

    private float speed;
    private float baseSpeed = 10.0f;
    private float lowSpeed = 5.0f;
    private float maxSpeed = 15.0f;
    public bool slowedDown = false;
    private float slowedTime = 5.0f;
    public bool poweredUp = false;
    public GameObject powerUpIndicator;
    private float powerTime = 7.0f;

    private float maxStamina = 100.0f;
    public float currentStamina;
    private float depleteRateStamina = 1.0f;
    public int score = 0;
    public int tokenCount = 0;

    private float foodValue = 30.0f;
    private int foodPoints = 5;
    private int tokenPoints = 10;
    private int powerUpPoints = 20;
    private int nimbyPoints = 40;
    private int nimbyStaminaDecrease = 15;
    private int nimbyTokenSteal = 2;

    private UIManager uiManagerScript;
    
    void Start()
    {
        // Set up the player
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        playerAnim.SetFloat("Speed_f", idleAnim);
        currentStamina = maxStamina;
        speed = baseSpeed;

        // Decrease stamina starting immediately
        InvokeRepeating("DepleteStamina", 0.00001f, depleteRateStamina);

        // Connect UI
        uiManagerScript = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void Update()
    {       
        // Check if the game is over
        CheckGameOver();
    }

    void FixedUpdate()
    {        
        // Move the player
        if (!GameManager.Instance.IsGameOver())
        {
            MovePlayerRelativeToCamera();
        }
    }

    void CheckGameOver()
    {
        // End the game if the player stamina is 0
        if (currentStamina <= 0)
        {
            GameManager.Instance.GameOver(true);

            playerAnim.SetFloat("Speed_f", idleAnim);
            Debug.Log("The NIMBYs were too powerful. You retire from your quest to make cities more sustainable, livable, and affordable. You finished with a score of " + score + ". Not bad, but you could do better!");
        }
    }

    void DepleteStamina()
    {
        currentStamina = currentStamina - depleteRateStamina;
        uiManagerScript.UpdateStaminaUI(currentStamina);
    }

    void MovePlayerRelativeToCamera()
    {
        // Get player input
        float playerVerticalInput = Input.GetAxis("Vertical");
        float playerHorizontalInput = Input.GetAxis("Horizontal");

        // Get camera-normalized directional vectors
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        // Create direction-relative input vectors
        Vector3 forwardRelativeVerticalInput = playerVerticalInput * forward;
        Vector3 rightRelativeHorizontalInput = playerHorizontalInput * right;

        // Create camera relative movement
        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;

        if (cameraRelativeMovement != Vector3.zero)
        {
            if (!slowedDown)
            {
                playerAnim.SetFloat("Speed_f", runAnim);
            }
            
            playerRb.MovePosition(transform.position + cameraRelativeMovement * speed * Time.deltaTime);

            // Rotate the player in accordance with player movement
            transform.rotation = Quaternion.LookRotation(cameraRelativeMovement);
        }
        else
        {
            playerAnim.SetFloat("Speed_f", idleAnim);
        }

        // Move the power up to follow the player
        powerUpIndicator.transform.position = transform.position + new Vector3(0, 0.1f, 0);
    }

    // Trigger events for food, tokens, and power ups
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food") && !GameManager.Instance.IsGameOver())
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

            Destroy(other.gameObject);
            Debug.Log("You eat some food.");
        }
        else if (other.CompareTag("Token") && !GameManager.Instance.IsGameOver())
        {
            tokenCount++;
            uiManagerScript.UpdateTokenUI(tokenCount);

            score += tokenPoints;
            uiManagerScript.UpdateScoreUI(score);

            Destroy(other.gameObject);
            Debug.Log("You pick up a token.");
        }
        else if (other.CompareTag("Power Up") && !GameManager.Instance.IsGameOver())
        {
            poweredUp = true;
            powerUpIndicator.gameObject.SetActive(true);
            StartCoroutine(PoweredUpCountRoutine());
            playerAnim.SetFloat("Speed_f", runAnim);

            score += powerUpPoints;
            uiManagerScript.UpdateScoreUI(score);

            speed = maxSpeed;
            Destroy(other.gameObject);
            Debug.Log("You power up!");
        }
    }

    // Trigger events for NIMBYs
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NIMBY") && !GameManager.Instance.IsGameOver())
        {
            Rigidbody nimbyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 nimbyAttack = collision.gameObject.transform.position - transform.position;
            nimbyRigidBody.AddForce(nimbyAttack, ForceMode.Impulse);

            // Power to destroy NIMBYs
            if (poweredUp == true)
            {
                score += nimbyPoints;
                uiManagerScript.UpdateScoreUI(score);

                Destroy(collision.gameObject);                
                Debug.Log("The NIMBY yielded to socially productive growth!");
            }
            // NIMBYs slow you down if you are not powered up but their effect does not stack
            else if (!slowedDown && !poweredUp)
            {
                slowedDown = true;
                speed = lowSpeed;
                StartCoroutine(SlowedDownCountRoutine());
                playerAnim.SetFloat("Speed_f", walkAnim);

                currentStamina -= nimbyStaminaDecrease;
                uiManagerScript.UpdateStaminaUI(currentStamina);

                tokenCount -= nimbyTokenSteal;
                uiManagerScript.UpdateTokenUI(tokenCount);

                if (tokenCount >= 0)
                {
                    Debug.Log("You collided with a NIMBY! They took some of your tokens!");
                }
                else if (tokenCount < 0)
                {
                    Debug.Log("You collided with a NIMBY! You are now in financial debt!");
                }
            }
        }
    }

    // The slow down lasts for 5 seconds
    IEnumerator SlowedDownCountRoutine()
    {
        yield return new WaitForSeconds(slowedTime);
        slowedDown = false;
        speed = baseSpeed;
        playerAnim.SetFloat("Speed_f", runAnim);
    }

    // The power up lasts for 7 seconds
    IEnumerator PoweredUpCountRoutine()
    {
        yield return new WaitForSeconds(powerTime);
        poweredUp = false;
        powerUpIndicator.gameObject.SetActive(false);
        speed = baseSpeed;
    }
}
