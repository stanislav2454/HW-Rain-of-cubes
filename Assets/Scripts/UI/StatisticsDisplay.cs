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

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _cubesInfoText;
    [SerializeField] private TextMeshProUGUI _bombsInfoText;

    [Header("System References")]
    [SerializeField] private GameController _gameController;
    [SerializeField] private ObjectPool<Cube> _cubePool;
    [SerializeField] private ObjectPool<Bomb> _bombPool;

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
        if (_gameController == null)
            Debug.LogError("GameController not set", this);
        if (_cubePool == null)
            Debug.LogError("CubePool not set", this);
        if (_bombPool == null)
            Debug.LogError("BombPool not set", this);
    }

    private void InitializeEventSubscriptions()
    {
        if (_gameController != null)
            _gameController.StatisticsChanged += OnStatisticsChanged;

        if (_cubePool != null)
            _cubePool.ObjectSpawned += OnStatisticsChanged;

        if (_bombPool != null)
            _bombPool.ObjectSpawned += OnStatisticsChanged;
    }

    private void UnsubscribeFromEvents()
    {
        if (_gameController != null)
            _gameController.StatisticsChanged -= OnStatisticsChanged;

        if (_cubePool != null)
            _cubePool.ObjectSpawned -= OnStatisticsChanged;

        if (_bombPool != null)
            _bombPool.ObjectSpawned -= OnStatisticsChanged;
    }

    private void OnStatisticsChanged<T>(T obj) =>
        UpdateAllDisplays();

    private void OnStatisticsChanged() =>
        UpdateAllDisplays();

    private void UpdateAllDisplays()
    {
        UpdatePoolDisplay(_cubePool, _cubesInfoText, CubesHeader, false);
        UpdatePoolDisplay(_bombPool, _bombsInfoText, BombsHeader, true);
        UpdateBombTextVisibility();
    }

    private void UpdatePoolDisplay<T>(ObjectPool<T> pool, TextMeshProUGUI textElement, string header, bool showExplosions) where T : MonoBehaviour, IPoolable
    {
        if (pool == null || textElement == null)
            return;

        _stringBuilder.Clear();
        _stringBuilder.AppendLine(header);
        _stringBuilder.AppendLine($"{ActiveLabel}{pool.ActiveCount}");
        _stringBuilder.AppendLine($"{InPoolLabel}{pool.InactiveCount}");

        if (showExplosions && _gameController != null)
        {
            _stringBuilder.AppendLine($"{TotalSpawnedLabel}{_gameController.TotalBombsSpawned}");
            _stringBuilder.AppendLine($"{ExplosionsLabel}{_gameController.TotalExplosions}");
        }
        else if (_gameController != null)
        {
            _stringBuilder.AppendLine($"{TotalSpawnedLabel}{_gameController.TotalCubesSpawned}");
        }

        textElement.text = _stringBuilder.ToString();
    }

    private void UpdateBombTextVisibility()
    {
        if (_bombsInfoText == null)
            return;

        bool shouldShow = (_bombPool?.ActiveCount ?? 0) > 0 ||
                         (_gameController?.TotalBombsSpawned ?? 0) > 0 ||
                         (_gameController?.TotalExplosions ?? 0) > 0;

        _bombsInfoText.gameObject.SetActive(shouldShow);
    }
}