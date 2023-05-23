using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NimbyController : MonoBehaviour
{
	private NavMeshAgent enemy;
	private Animator enemyAnim;
	private float speed = 3.0f;
	private float walkAnim = 0.3f;
	private float idleAnim = 0f;

	public Transform playerTarget;

	private void Start()
	{
		enemy = GetComponent<NavMeshAgent>();
		enemy.speed = speed;

		enemyAnim = GetComponentInChildren<Animator>();
	}

	private void FixedUpdate()
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
		
		enemy.destination = playerTarget.position;
	}
}