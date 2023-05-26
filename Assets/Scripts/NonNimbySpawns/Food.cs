using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Food is a class of poolable object
public class Food : AutoDestroyPoolableObject
{
    // Assign a new AutoDestroyTime compared to the parent class AutoDestroyPoolableObject.cs
    public float AutoDestroyTimeFood = 90f;

    // Override the OnEnable() in AutoDestroyPoolableObject.cs to change the AutoDestroyTime
    public override void OnEnable()
    {
        // Cancel any previous invoke so that all the poolable objects are not disabled at the same time
        CancelInvoke(DisableMethodName);

        // Start a new invoke for each poolable object when it is enabled
        Invoke(DisableMethodName, AutoDestroyTimeFood);
    }

    public override void OnDisable()
    {
        // Return the food to its pool when it is disabled (PoolableObject.cs)
        base.OnDisable();
    }
}
