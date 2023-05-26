public class AutoDestroyPoolableObject : PoolableObject
{
    public float AutoDestroyTime = 140f;

    private const string DisableMethodName = "Disable";

    public virtual void OnEnable()
    {
        // Cancel any previous invoke so that all the poolable objects are not disabled at the same time
        CancelInvoke(DisableMethodName);

        // Start a new invoke for each poolable object when it is enabled
        Invoke(DisableMethodName, AutoDestroyTime);
    }

    public virtual void Disable()
    {
        // Disable the game object, which will then have PoolableObject.cs return it to the pool
        gameObject.SetActive(false);
    }
}