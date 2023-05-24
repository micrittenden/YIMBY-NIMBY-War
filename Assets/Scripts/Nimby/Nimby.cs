using UnityEngine.AI;

public class Nimby : AutoDestroyPoolableObject
{
    public NimbyController movement;
    public NavMeshAgent agent;
    public NimbyScriptableObject nimbyScriptableObject;

    public virtual void OnEnable()
    {
        SetupAgentFromConfiguration();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        agent.enabled = false;
    }

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