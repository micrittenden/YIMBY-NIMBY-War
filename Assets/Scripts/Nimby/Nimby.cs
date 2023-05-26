using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
// NIMBY is a class of poolable object
public class Nimby : AutoDestroyPoolableObject
{
    // All assigned in the inspector
    public NimbyController movement;
    public NavMeshAgent agent;
    public NimbyScriptableObject nimbyScriptableObject;

    public override void OnEnable()
    {
        // Invoke the NIMBY to be returned to the pool after a specified time if it is not already hit by the player by then (AutoDestroyPoolableObject.cs)
        base.OnEnable();

        // Set up the NIMBY's NavMeshAgent each time it is spawned
        SetupAgentFromConfiguration();
    }

    public override void OnDisable()
    {
        // Return the NIMBY to its pool when it is disabled (PoolableObject.cs)
        base.OnDisable();

        // Reset followCoroutine to null in NimbyController.cs so that the NIMBY will StartChasing() again when respawned
        movement.followCoroutine = null;

        // Disable the NIMBY's NavMeshAgent so that it does not spawn in the same place the next time
        agent.enabled = false;
    }

    // Set up configs for the NIMBY's NavMeshAgent component
    public virtual void SetupAgentFromConfiguration()
    {
    agent.baseOffset = nimbyScriptableObject.BaseOffset;
	agent.speed = nimbyScriptableObject.Speed;
	agent.angularSpeed = nimbyScriptableObject.AngularSpeed;
	agent.acceleration = nimbyScriptableObject.Acceleration;
	agent.stoppingDistance = nimbyScriptableObject.StoppingDistance;
	agent.radius = nimbyScriptableObject.Radius;
	agent.height = nimbyScriptableObject.Height;
	agent.obstacleAvoidanceType = nimbyScriptableObject.ObstacleAvoidanceType;
	agent.avoidancePriority = nimbyScriptableObject.AvoidancePriority;
	agent.areaMask = nimbyScriptableObject.AreaMask;
    movement.updateRate = nimbyScriptableObject.AIUpdateInterval;
    }
}