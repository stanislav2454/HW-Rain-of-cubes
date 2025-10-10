using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private const float Delay = 1f;

    [SerializeField] private ObjectPool<Cube> _cubePool;
    [SerializeField] private ObjectPool<Bomb> _bombPool;
    [SerializeField] private float _cubeSpawnInterval = 1f;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _spawnHeight = 15f;

    private int _totalCubesSpawned = 0;
    private int _totalBombsSpawned = 0;
    private int _totalExplosions = 0;

    public event System.Action StatisticsChanged;

    public int TotalCubesSpawned => _totalCubesSpawned;
    public int TotalBombsSpawned => _totalBombsSpawned;
    public int TotalExplosions => _totalExplosions;

    private void Start()
    {
        ValidateReferences();
        StartCoroutine(CubeSpawningRoutine());
        SetupEventHandlers();
    }

    private void ValidateReferences()
    {
        if (_cubePool == null)
            Debug.LogError("CubePool not set in GameController", this);

        if (_bombPool == null)
            Debug.LogError("BombPool not set in GameController", this);
    }

    private void OnDestroy()
    {
        if (_cubePool != null)
        {
            _cubePool.ObjectSpawned -= OnCubeSpawned;
            _cubePool.ObjectDespawned -= OnCubeDespawned;
        }

        if (_bombPool != null)
        {
            _bombPool.ObjectSpawned -= OnBombSpawned;
            _bombPool.ObjectDespawned -= OnBombDespawned;
        }
    }

    private void SetupEventHandlers()
    {
        if (_cubePool != null)
        {
            _cubePool.ObjectSpawned += OnCubeSpawned;
            _cubePool.ObjectDespawned += OnCubeDespawned;
        }

        if (_bombPool != null)
        {
            _bombPool.ObjectSpawned += OnBombSpawned;
            _bombPool.ObjectDespawned += OnBombDespawned;
        }
    }

    private IEnumerator CubeSpawningRoutine()
    {
        yield return new WaitForSeconds(Delay);

        while (true)
        {
            yield return new WaitForSeconds(_cubeSpawnInterval);
            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        if (_cubePool == null)
            return;

        try
        {
            var cube = _cubePool.Spawn();
            if (cube != null)
            {
                cube.transform.position = GetRandomSpawnPosition();
                cube.SetDestroyCallback(OnCubeDestroyed);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error spawning cube: {ex.Message}");
        }
    }

    private void SpawnBomb(Vector3 position)
    {
        if (_bombPool == null)
            return;

        try
        {
            var bomb = _bombPool.Spawn();
            if (bomb != null)
            {
                bomb.transform.position = position;
                bomb.SetExplodeCallback(OnBombExploded);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error spawning bomb: {ex.Message}");
        }
    }

    private Vector3 GetRandomSpawnPosition() => new Vector3(
        Random.Range(-_spawnRadius, _spawnRadius),
        _spawnHeight,
        Random.Range(-_spawnRadius, _spawnRadius));

    private void OnCubeSpawned(Cube cube) =>
        _totalCubesSpawned++;
    private void OnBombSpawned(Bomb bomb) =>
        _totalBombsSpawned++;
    private void OnBombDespawned(Bomb bomb) =>
        _totalExplosions++;

    private void OnCubeDestroyed(Cube cube)
    {
        if (_cubePool != null)
        {
            _cubePool.Despawn(cube);
            SpawnBomb(cube.transform.position);
            NotifyStatisticsChanged();
        }
    }

    private void OnBombExploded(Bomb bomb)
    {
        if (_bombPool != null)
        {
            _bombPool.Despawn(bomb);
            NotifyStatisticsChanged();
        }
    }

    private void OnCubeDespawned(Cube cube) =>
        NotifyStatisticsChanged();

    private void NotifyStatisticsChanged() =>
        StatisticsChanged?.Invoke();
}