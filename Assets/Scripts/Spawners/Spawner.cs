using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T: MonoBehaviour
{
    [SerializeField] protected T Prefab;

    protected GameObject ObjectsContainer;

    protected ObjectPool<T> Pool;
    protected int PoolCapacity;
    protected int PoolSize;

    public event Action InstanceCreated;
    public event Action<int, int> PoolChanged;

    protected virtual void Awake()
    {
        PoolCapacity = 15;
        PoolSize = 10;

        ObjectsContainer =  new GameObject(typeof(T) + "Container");

        Pool = new ObjectPool<T>(
            createFunc: () => Instantiate(Prefab, ObjectsContainer.transform),
            actionOnGet: (instance) => OnGetInstance(instance),
            actionOnRelease: (instance) => OnReleaseInstance(instance),
            actionOnDestroy: (instance) => Destroy(instance),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: PoolSize);
    }

    protected virtual void OnGetInstance(T instance)
    {
        instance.gameObject.SetActive(true);

        InstanceCreated?.Invoke();
        PoolChanged?.Invoke(Pool.CountAll, Pool.CountActive);
    }

    protected void GetInstance()
    {
        Pool.Get();
    }

    protected virtual void OnReleaseInstance(T instance)
    {
        instance.gameObject.SetActive(false);
    }

    protected void Release(T instance)
    {
        Pool.Release(instance);
        PoolChanged?.Invoke(Pool.CountAll, Pool.CountActive);
    }
}
