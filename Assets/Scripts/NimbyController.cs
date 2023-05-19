using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimbyController : MonoBehaviour
{
    private float speed = 3.0f;
    private Rigidbody enemyRb;
    private Animator enemyAnim;
    private float walkAnim = 0.3f;
    private float idleAnim = 0f;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        enemyAnim = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            MoveNimby();
        }
        else
        {
            enemyAnim.SetFloat("Speed_f", idleAnim);
        }
    }

    void MoveNimby()
    {
        enemyAnim.SetFloat("Speed_f", walkAnim);

        Vector3 moveDirection = (player.transform.position - transform.position).normalized;

        enemyRb.MovePosition(transform.position + (moveDirection * speed *Time.deltaTime));

        transform.rotation = Quaternion.LookRotation(moveDirection);
    }
}
