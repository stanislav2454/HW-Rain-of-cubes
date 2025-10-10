using UnityEngine;
using System.Text;
using TMPro;

public class StatisticsDisplay : MonoBehaviour
{
    private const string CubesHeader = "=== CUBES ===";
    private const string BombsHeader = "=== BOMBS ===";
    private const string ActiveLabel = "Active: ";
    private const string InPoolLabel = "In Pool: ";
    private const string TotalSpawnedLabel = "Total Spawned: ";
    private const string ExplosionsLabel = "Explosions: ";
    private const string PoolNameCube = "Cube";
    private const string PoolNameBomb = "Bomb";

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _cubesInfoText;
    [SerializeField] private TextMeshProUGUI _bombsInfoText;

    [Header("System References")]
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private SimplePool _cubePool;
    [SerializeField] private SimplePool _bombPool;
    [SerializeField] private StatisticsManager _statisticsManager;

    private StringBuilder _stringBuilder = new StringBuilder();

    private void Start()
    {
        ValidateReferences();
        InitializeEventSubscriptions();
        UpdateAllDisplays();
    }

    private void OnDestroy() =>
        UnsubscribeFromEvents();

    private void Update() =>
        UpdateAllDisplays();

    private void ValidateReferences()
    {
        if (_cubeSpawner == null)
            Debug.LogError($"CubeSpawner not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombSpawner == null)
            Debug.LogError($"BombSpawner not set for {GetType().Name} on {gameObject.name}", this);

        if (_cubePool == null)
            Debug.LogError($"CubePool not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombPool == null)
            Debug.LogError($"BombPool not set for {GetType().Name} on {gameObject.name}", this);

        if (_statisticsManager == null)
            _statisticsManager = FindObjectOfType<StatisticsManager>();//переделать FindObjectOfType
    }

    private void InitializeEventSubscriptions()
    {
        if (_cubeSpawner != null)
            _cubeSpawner.CubeDestroyed += OnCubeDestroyed;

        if (_bombSpawner != null)
        {
            _bombSpawner.BombSpawned += OnBombSpawned;
            _bombSpawner.BombExploded += OnBombExploded;
        }

        if (_statisticsManager != null)
            _statisticsManager.StatisticsChanged += OnStatisticsChanged;

        if (_cubePool != null)
            _cubePool.PoolCountChanged += OnStatisticsChanged;

        if (_bombPool != null)
            _bombPool.PoolCountChanged += OnStatisticsChanged;
    }

    private void UnsubscribeFromEvents()
    {
        if (_cubeSpawner != null)
            _cubeSpawner.CubeDestroyed -= OnCubeDestroyed;

        if (_bombSpawner != null)
        {
            _bombSpawner.BombSpawned -= OnBombSpawned;
            _bombSpawner.BombExploded -= OnBombExploded;
        }

        if (_statisticsManager != null)
            _statisticsManager.StatisticsChanged -= OnStatisticsChanged;

        if (_cubePool != null)
            _cubePool.PoolCountChanged -= OnStatisticsChanged;

        if (_bombPool != null)
            _bombPool.PoolCountChanged -= OnStatisticsChanged;
    }

    private void OnCubeDestroyed(Vector3 _) =>//зачем аргумент ?
        _statisticsManager?.IncrementCubesSpawned();

    private void OnBombSpawned() =>
        _statisticsManager?.IncrementBombsSpawned();

    private void OnBombExploded() =>
        _statisticsManager?.IncrementExplosions();

    private void OnStatisticsChanged() =>
        UpdateAllDisplays();

    private void UpdateAllDisplays()
    {
        UpdatePoolDisplay(_cubePool, _cubesInfoText, CubesHeader, false);
        UpdatePoolDisplay(_bombPool, _bombsInfoText, BombsHeader, true);
        UpdateBombTextVisibility();
    }

    private void UpdatePoolDisplay(SimplePool pool, TextMeshProUGUI textElement, string header, bool showExplosions)
    {
        if (pool == null || textElement == null) return;

        string poolName = pool.PoolName;
        int activeCount = pool.GetActiveCount(poolName);
        int inactiveCount = pool.GetPoolCount(poolName);

        _stringBuilder.Clear();
        _stringBuilder.AppendLine(header);
        _stringBuilder.AppendLine($"{ActiveLabel}{activeCount}");
        _stringBuilder.AppendLine($"{InPoolLabel}{inactiveCount}");

        if (showExplosions && _statisticsManager != null)
        {
            _stringBuilder.AppendLine($"{TotalSpawnedLabel}{_statisticsManager.TotalBombsSpawned}");
            _stringBuilder.AppendLine($"{ExplosionsLabel}{_statisticsManager.TotalExplosions}");
        }
        else if (_statisticsManager != null)
        {
            _stringBuilder.AppendLine($"{TotalSpawnedLabel}{_statisticsManager.TotalCubesSpawned}");
        }

        textElement.text = _stringBuilder.ToString();
    }

    private void UpdateBombTextVisibility()
    {
        if (_bombsInfoText == null)
            return;

        bool shouldShow = (_bombPool?.GetActiveCount(PoolNameBomb) ?? 0) > 0 ||
                         (_statisticsManager?.TotalBombsSpawned ?? 0) > 0 ||
                         (_statisticsManager?.TotalExplosions ?? 0) > 0;
        
        _bombsInfoText.gameObject.SetActive(shouldShow);
    }

    public void ForceUpdate()
    {
        UpdateAllDisplays();
    }
}