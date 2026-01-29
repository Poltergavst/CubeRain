using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T: MonoBehaviour
{
    [SerializeField] protected T _prefab;

    protected GameObject _objectsContainer;

    protected ObjectPool<T> _pool;
    protected int _poolCapacity;
    protected int _poolSize;

    public event Action InstanceCreated;
    public event Action<int, int> PoolChanged;

    protected virtual void Awake()
    {
        _poolCapacity = 15;
        _poolSize = 10;

        _objectsContainer =  new GameObject(typeof(T) + "Container");

        _pool = new ObjectPool<T>(
            createFunc: () => Instantiate(_prefab, _objectsContainer.transform),
            actionOnGet: (instance) => OnGetInstance(instance),
            actionOnRelease: (instance) => OnReleaseInstance(instance),
            actionOnDestroy: (instance) => Destroy(instance),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolSize);
    }

    protected virtual void OnGetInstance(T instance)
    {
        instance.gameObject.SetActive(true);

        InstanceCreated?.Invoke();
        PoolChanged?.Invoke(_pool.CountAll, _pool.CountActive);
    }

    protected void GetInstance()
    {
        _pool.Get();
    }

    protected virtual void OnReleaseInstance(T instance)
    {
        instance.gameObject.SetActive(false);
    }

    protected void Release(T instance)
    {
        _pool.Release(instance);
        PoolChanged?.Invoke(_pool.CountAll, _pool.CountActive);
    }
}
