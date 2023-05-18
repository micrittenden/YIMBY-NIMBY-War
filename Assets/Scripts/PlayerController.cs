using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;

    // Player animation
    private Animator playerAnim;
    private float idleAnim = 0f;
    private float walkAnim = 0.3f;
    private float runAnim = 0.6f;

    // Player speed and capabilities
    private float speed;
    private float baseSpeed = 10.0f;
    private float lowSpeed = 5.0f;
    private float maxSpeed = 15.0f;
    public bool slowedDown = false;
    private float slowedTime = 5.0f;
    public bool poweredUp = false;
    public GameObject powerUpIndicator;
    private float powerTime = 7.0f;
    
    void Start()
    {
        // Set up the player
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        playerAnim.SetFloat("Speed_f", idleAnim);
        speed = baseSpeed;
    }

    void FixedUpdate()
    {        
        // Move the player
        if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            MovePlayerRelativeToCamera();
        }
        else
        {
            playerAnim.SetFloat("Speed_f", idleAnim);
        }
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
        if (other.CompareTag("Food") && GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            GameManager.Instance.EatFood();

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Token") && GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            GameManager.Instance.PickUpToken();

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Power Up") && GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            poweredUp = true;
            powerUpIndicator.gameObject.SetActive(true);
            StartCoroutine(PoweredUpCountRoutine());
            playerAnim.SetFloat("Speed_f", runAnim);
            speed = maxSpeed;

            GameManager.Instance.PowerUp();

            Destroy(other.gameObject);
        }
    }

    // Trigger events for NIMBYs
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NIMBY") && GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            // Power to destroy NIMBYs
            if (poweredUp == true)
            {
                GameManager.Instance.DestroyNimby();

                Destroy(collision.gameObject);
            }
            // NIMBYs slow you down if you are not powered up but their effect does not stack
            else if (!slowedDown && !poweredUp)
            {
                slowedDown = true;
                speed = lowSpeed;
                StartCoroutine(SlowedDownCountRoutine());
                playerAnim.SetFloat("Speed_f", walkAnim);

                GameManager.Instance.AttackedByNimby();
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
