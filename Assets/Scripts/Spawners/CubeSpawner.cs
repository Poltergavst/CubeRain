using System;
using System.Collections;
using UnityEngine;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private Collider _spawnBounds;

    private float _spawnRepeatRate;

    public event Action<Cube> CubeReleased;

    protected override void Awake()
    {
        _spawnRepeatRate = 0.5f;

        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(SpawnRepeatedly());
    }

    protected override void OnGetInstance(Cube cube)
    {
        cube.transform.position = GetSpawnPosition();

        base.OnGetInstance(cube);

        cube.GotReleased += Release;
    }

    protected override void OnReleaseInstance(Cube cube)
    {
        cube.GotReleased -= Release;

        base.OnReleaseInstance(cube);

        CubeReleased?.Invoke(cube);
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;

        float leftSpawnBound = _spawnBounds.bounds.min.x;
        float rightSpawnBound = _spawnBounds.bounds.max.x;
        float upperSpawnPosition = 5;

        spawnPosition.y = upperSpawnPosition;
        spawnPosition.z = _spawnBounds.bounds.center.z;
        spawnPosition.x = UnityEngine.Random.Range(leftSpawnBound, rightSpawnBound);

        return spawnPosition;
    }

    private IEnumerator SpawnRepeatedly()
    { 
        WaitForSeconds delay = new(_spawnRepeatRate);

        while (isActiveAndEnabled)
        {
            GetInstance();

            yield return delay;
        }
    }
}
