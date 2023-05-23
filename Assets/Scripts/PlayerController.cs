using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private float kMinMoveDistance = 0.01f;
    private float kMaxDistFromNavMesh = 0.3f;

    // Player animation
    private Animator playerAnim;
    private float idleAnim = 0f;
    private float walkAnim = 0.3f;
    private float runAnim = 0.6f;

    // Player speed and capabilities
    private float speed;
    private float baseSpeed = 6.0f;
    private float lowSpeed = 3.0f;
    private float maxSpeed = 9.0f;
    private bool slowedDown;
    private float slowedTime = 5.0f;
    private bool poweredUp;
    private float powerTime = 30.0f;
    public GameObject powerUpIndicator;

    void Start()
    {
        // Set up the player
        speed = baseSpeed;
        playerAnim = GetComponentInChildren<Animator>();
        playerAnim.SetFloat("Speed_f", idleAnim);
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

    private void MovePlayerRelativeToCamera()
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
            else if (slowedDown)
            {
                playerAnim.SetFloat("Speed_f", walkAnim);
            }

            Vector3 inputMoveDelta = cameraRelativeMovement * speed * Time.deltaTime;
            Vector3 desiredPosition = transform.position + inputMoveDelta;

            // Check if the desired position produces a valid movement
            NavMeshHit hit;
            bool isValid = NavMesh.SamplePosition(desiredPosition, out hit, kMaxDistFromNavMesh, NavMesh.AllAreas);
            if (isValid)
            {
                // Check if it is enough movement
                if (Vector3.Distance(transform.position, hit.position) > kMinMoveDistance)
                {
                    transform.position = hit.position;
                    transform.rotation = Quaternion.LookRotation(cameraRelativeMovement);
                }
            }
        }
        else
        {
            // No input from player
            playerAnim.SetFloat("Speed_f", idleAnim);
        }

        // Move the power up indicator to follow the player
        powerUpIndicator.transform.position = transform.position + new Vector3(0, 0.1f, 0);
        powerUpIndicator.transform.RotateAround(transform.position, new Vector3(0, -1, 0), 90 * Time.deltaTime);
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
            slowedDown = false;
            poweredUp = true;
            powerUpIndicator.gameObject.SetActive(true);
            speed = maxSpeed;

            GameManager.Instance.PowerUp();

            StartCoroutine(PoweredUpCountRoutine());

            Destroy(other.gameObject);
        }
    }

    // Trigger events for NIMBYs
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NIMBY") && GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            // Power to destroy NIMBYs
            if (poweredUp)
            {
                GameManager.Instance.DestroyNimby();

                Destroy(collision.gameObject);
            }
            // NIMBYs attack you if you are not powered up but their effect does not stack
            else if (!slowedDown && !poweredUp)
            {
                slowedDown = true;
                speed = lowSpeed;

                GameManager.Instance.AttackedByNimby();

                StartCoroutine(SlowedDownCountRoutine());
            }
        }
    }

    // The slow down lasts for 5 seconds
    IEnumerator SlowedDownCountRoutine()
    {
        yield return new WaitForSeconds(slowedTime);
        slowedDown = false;
        speed = baseSpeed;
    }

    // The power up lasts for 30 seconds
    IEnumerator PoweredUpCountRoutine()
    {
        yield return new WaitForSeconds(powerTime);
        poweredUp = false;
        powerUpIndicator.gameObject.SetActive(false);
        speed = baseSpeed;
    }
}
