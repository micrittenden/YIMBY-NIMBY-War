using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool Parent;

    // Return an object to its pool when it is disabled
    public virtual void OnDisable()
    {
        Parent.ReturnObjectToPool(this);
    }
}