using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Spawner References")]
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;

    [Header("Pool References")]
    [SerializeField] private ObjectPool<Cube> _cubePool;
    [SerializeField] private ObjectPool<Bomb> _bombPool;

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
        SetupEventHandlers();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void ValidateReferences()
    {
        if (_cubeSpawner == null)
            Debug.LogError("CubeSpawner not set in GameController", this);

        if (_bombSpawner == null)
            Debug.LogError("BombSpawner not set in GameController", this);

        if (_cubePool == null)
            Debug.LogError("CubePool not set in GameController", this);

        if (_bombPool == null)
            Debug.LogError("BombPool not set in GameController", this);
    }

    private void SetupEventHandlers()
    {
        if (_cubeSpawner != null)
            _cubeSpawner.CubeDestroyed += OnCubeDestroyed;

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

    private void UnsubscribeFromEvents()
    {
        if (_cubeSpawner != null)
            _cubeSpawner.CubeDestroyed -= OnCubeDestroyed;

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

    private void OnCubeDestroyed(Cube cube)
    {
        _bombSpawner.SpawnBomb(cube.transform.position);
        NotifyStatisticsChanged();
    }

    private void OnBombSpawned(Bomb _) =>
        _totalBombsSpawned++;

    private void OnCubeSpawned(Cube _) =>
        _totalCubesSpawned++;

    private void OnBombDespawned(Bomb _) =>
        _totalExplosions++;

    private void OnCubeDespawned(Cube _) =>
        NotifyStatisticsChanged();

    private void NotifyStatisticsChanged() =>
        StatisticsChanged?.Invoke();
}