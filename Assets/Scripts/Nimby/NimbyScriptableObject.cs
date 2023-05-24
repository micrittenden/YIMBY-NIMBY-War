using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Nimby Configuration", menuName = "ScriptableObject/Nimby Configuration")]
public class NimbyScriptableObject : ScriptableObject
{
	public float AIUpdateInterval = 0.1f;

	public float BaseOffset = 0;
	public float Speed = 3.5f;
	public float AngularSpeed = 120;
	public float Acceleration = 8;
	public float StoppingDistance = 0f;

	public float Radius = 0.5f;
	public float Height = 1f;
	public ObstacleAvoidanceType ObstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
	public int AvoidancePriority = 50;

	public int AreaMask = 1;
}
