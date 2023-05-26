using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NimbyController : MonoBehaviour
{
	public Transform player;
	private NavMeshAgent nimby;
	private Animator nimbyAnim;
	public float updateRate = 0.1f;
	private float speed = 4.0f;
	private float walkAnim = 0.3f;
	private float idleAnim = 0f;

	private Coroutine followCoroutine;

	// Set up the NIMBY
	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;

		nimby = GetComponent<NavMeshAgent>();
		nimbyAnim = GetComponentInChildren<Animator>();
	}

	// Chase the player if not already chasing the player
	public void StartChasing()
	{
		if (followCoroutine == null)
		{
			followCoroutine = StartCoroutine(FollowTarget());
		}
	}

	// Set destination, speed, animation, and destination update rate for chasing the player
	private IEnumerator FollowTarget()
	{
		WaitForSeconds Wait = new WaitForSeconds(updateRate);

		while (gameObject.activeSelf)
		{
			if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
			{
				nimby.speed = speed;
				nimbyAnim.SetFloat("Speed_f", walkAnim);

				nimby.SetDestination(player.transform.position);
			}
			else
			{
				nimby.speed = 0f;
				nimbyAnim.SetFloat("Speed_f", idleAnim);
			}
			yield return Wait;
		}
	}
}