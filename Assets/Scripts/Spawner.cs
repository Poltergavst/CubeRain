using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Collider _spawnBounds;
    [SerializeField] private Cube _cubePrefab;

    private GameObject _cubesContainer;

    private float _leftSpawnBound;
    private float _rightSpawnBound;
    private float _upperSpawnPosition;

    private float _spawnStartTime;
    private float _spawnRepeatRate;

    private ObjectPool<Cube> _pool;
    private int _poolCapacity;
    private int _poolSize;

    private void Awake()
    {
        _poolCapacity = 15;
        _poolSize = 10;

        _upperSpawnPosition = 5;
        _leftSpawnBound = _spawnBounds.bounds.min.x;
        _rightSpawnBound = _spawnBounds.bounds.max.x;

        _spawnStartTime = 0f;
        _spawnRepeatRate = 0.5f;

        _cubesContainer =  new GameObject("Cubes");

        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab, _cubesContainer.transform),
            actionOnGet: (cube) => OnGetCube(cube),
            actionOnRelease: (cube) => OnReleaseCube(cube),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), _spawnStartTime, _spawnRepeatRate);
    }

    private void OnGetCube(Cube cube)
    {
        cube.transform.position = GetSpawnPosition();
        cube.gameObject.SetActive(true);
        cube.GotReleased += Release;
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void OnReleaseCube(Cube cube)
    {
        cube.GotReleased -= Release;
        cube.gameObject.SetActive(false);
    }

    private void Release(Cube cube)
    {
        _pool.Release(cube);
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition;
        
        spawnPosition.y = _upperSpawnPosition;
        spawnPosition.z = _spawnBounds.bounds.center.z;
        spawnPosition.x = Random.Range(_leftSpawnBound, _rightSpawnBound);

        return spawnPosition;
    }
}
