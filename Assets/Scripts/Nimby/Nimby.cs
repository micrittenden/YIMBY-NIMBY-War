using UnityEngine.AI;

public class Nimby : AutoDestroyPoolableObject
{
    public NimbyController movement;
    public NavMeshAgent agent;
    public NimbyScriptableObject nimbyScriptableObject;

    // Set up the NIMBY's NavMeshAgent each time it is enabled (ie, spawned)
    public virtual void OnEnable()
    {
        SetupAgentFromConfiguration();
    }

    // Disable the NIMBY's NavMeshAgent so that it does not spawn in the same place the next time
    public override void OnDisable()
    {
        base.OnDisable();

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