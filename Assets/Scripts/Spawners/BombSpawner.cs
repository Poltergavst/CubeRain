using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private Vector3 _spawnPosition;

    protected override void Awake()
    {
        base.Awake();

        _cubeSpawner.CubeReleased += SpawnOnCubePlace;
    }

    protected override void OnGetInstance(Bomb bomb)
    {
        bomb.Detonated += Release;
        bomb.transform.position = _spawnPosition;

        base.OnGetInstance(bomb);
    }

    protected override void OnReleaseInstance(Bomb bomb)
    {
        bomb.Detonated -= Release;
        base.OnReleaseInstance(bomb);
    }

    private void SpawnOnCubePlace(Cube cube)
    {
        _spawnPosition = cube.transform.position;

        GetInstance();
    }
}
