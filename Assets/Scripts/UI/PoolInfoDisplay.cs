using UnityEngine;
using System.Text;
using TMPro;

public class PoolInfoDisplay : MonoBehaviour
{
    private const string CubesHeader = "=== CUBES ===";
    private const string BombsHeader = "=== BOMBS ===";
    private const string ActiveLabel = "Active: ";
    private const string InPoolLabel = "In Pool: ";
    private const string TotalLabel = "Total: ";
    private const string TotalSpawnedLabel = "Total Spawned: ";
    private const string ExplosionsLabel = "Explosions: ";

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _cubesInfoText;
    [SerializeField] private TextMeshProUGUI _bombsInfoText;

    [Header("Pool References")]
    [SerializeField] private CubePool _cubePool;
    [SerializeField] private BombPool _bombPool;
    [SerializeField] private BombSpawner _bombSpawner;

    private StringBuilder _stringBuilder = new StringBuilder();
    private bool _needsCubeUpdate = true;
    private bool _needsBombUpdate = true;
    private bool _needsVisibilityUpdate = true;

    private void Start()
    {
        ValidateReferences();
        InitializeEventSubscriptions();
        ForceUpdateAll();
    }

    private void OnDestroy() =>
        UnsubscribeFromEvents();

    private void Update()
    {
        if (_needsCubeUpdate)
        {
            UpdatePoolDisplay(_cubePool, _cubesInfoText, CubesHeader, showExplosions: false);
            _needsCubeUpdate = false;
        }

        if (_needsBombUpdate)
        {
            UpdatePoolDisplay(_bombPool, _bombsInfoText, BombsHeader, showExplosions: true);
            _needsBombUpdate = false;
        }

        if (_needsVisibilityUpdate)
        {
            UpdateBombTextVisibility();
            _needsVisibilityUpdate = false;
        }
    }

    private void OnCubeActiveCountChanged(int count) =>
        _needsCubeUpdate = true;

    private void OnCubeSpawnedCountChanged(int count) =>
        _needsCubeUpdate = true;

    private void OnBombActiveCountChanged(int count)
    {
        _needsBombUpdate = true;
        _needsVisibilityUpdate = true;
    }

    private void OnBombSpawnedCountChanged(int count) =>
        _needsBombUpdate = true;

    private void ValidateReferences()
    {
        if (_cubePool == null)
            Debug.LogError($"CubePool not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombPool == null)
            Debug.LogError($"BombPool not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombSpawner == null)
            Debug.LogError($"BombSpawner not set for {GetType().Name} on {gameObject.name}", this);
    }

    private void InitializeEventSubscriptions()
    {
        if (_cubePool != null)
        {
            _cubePool.OnActiveCountChanged += OnCubeActiveCountChanged;
            _cubePool.OnSpawnedCountChanged += OnCubeSpawnedCountChanged;
            _cubePool.ForceNotifyAll();
        }

        if (_bombPool != null)
        {
            _bombPool.OnActiveCountChanged += OnBombActiveCountChanged;
            _bombPool.OnSpawnedCountChanged += OnBombSpawnedCountChanged;
            _bombPool.ForceNotifyAll();
        }

        if (_bombSpawner != null)
            _bombSpawner.OnSpawnerStateChanged += OnBombSpawnerStateChanged;

        ExplosiveObject.OnAnyExplosion += OnAnyExplosion;
    }

    private void UnsubscribeFromEvents()
    {
        if (_cubePool != null)
        {
            _cubePool.OnActiveCountChanged -= OnCubeActiveCountChanged;
            _cubePool.OnSpawnedCountChanged -= OnCubeSpawnedCountChanged;
        }

        if (_bombPool != null)
        {
            _bombPool.OnActiveCountChanged -= OnBombActiveCountChanged;
            _bombPool.OnSpawnedCountChanged -= OnBombSpawnedCountChanged;
        }

        if (_bombSpawner != null)
            _bombSpawner.OnSpawnerStateChanged -= OnBombSpawnerStateChanged;

        ExplosiveObject.OnAnyExplosion -= OnAnyExplosion;
    }

    private void OnBombSpawnerStateChanged(bool isSpawning)
    {
        _needsVisibilityUpdate = true;
        Debug.Log($"Bomb spawner state changed: {(isSpawning ? "ENABLED" : "DISABLED")}");
    }

    private void OnAnyExplosion()
    {
        _needsBombUpdate = true;
        _needsVisibilityUpdate = true;
    }

    private void ForceUpdateAll()
    {
        _needsCubeUpdate = true;
        _needsBombUpdate = true;
        _needsVisibilityUpdate = true;
    }

    private void UpdatePoolDisplay(IPoolInfo pool, TextMeshProUGUI textElement, string header, bool showExplosions)
    {
        if (pool == null || textElement == null)
            return;

        _stringBuilder.Clear();
        _stringBuilder.AppendLine(header);
        _stringBuilder.AppendLine($"{ActiveLabel}{pool.ActiveCount}");
        _stringBuilder.AppendLine($"{InPoolLabel}{pool.InactiveCount}");

        if (showExplosions)
        {
            _stringBuilder.AppendLine($"{TotalSpawnedLabel}{pool.SpawnedCount}");
            _stringBuilder.AppendLine($"{ExplosionsLabel}{ExplosiveObject.TotalExplosions}");
        }
        else
        {
            _stringBuilder.AppendLine($"{TotalLabel}{pool.SpawnedCount}");
        }

        textElement.text = _stringBuilder.ToString();
    }

    private void UpdateBombTextVisibility()
    {
        if (_bombsInfoText == null)
            return;

        bool shouldShow = (_bombSpawner != null && _bombSpawner.IsSpawning) ||
                          (_bombPool != null && _bombPool.ActiveCount > 0) ||
                          (ExplosiveObject.TotalExplosions > 0);

        _bombsInfoText.gameObject.SetActive(shouldShow);
    }
}