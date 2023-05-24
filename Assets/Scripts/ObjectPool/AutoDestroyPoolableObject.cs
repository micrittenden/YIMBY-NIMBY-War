public class AutoDestroyPoolableObject : PoolableObject
{
    public float AutoDestroyTime = 140f;

    private const string DisableMethodName = "Disable";

    public virtual void OnEnable()
    {
        CancelInvoke(DisableMethodName);

        Invoke(DisableMethodName, AutoDestroyTime);
    }

    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }
}