using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Nimby : AutoDestroyPoolableObject
{
    public Rigidbody RigidBody;
    public Vector3 Speed = new Vector3(5, 0, -1);

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        RigidBody.velocity = Speed;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        RigidBody.velocity = Vector3.zero;
    }
}