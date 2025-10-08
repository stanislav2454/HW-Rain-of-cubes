using UnityEngine;
using System.Text;
using TMPro;

public class PoolInfoDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _cubesInfoText;
    [SerializeField] private TextMeshProUGUI _bombsInfoText;

    [Header("Pool References")]
    [SerializeField] private CubePool _cubePool;
    [SerializeField] private BombPool _bombPool;
    [SerializeField] private BombSpawner _bombSpawner;

    private StringBuilder _stringBuilder = new StringBuilder();

    private void Start()
    {
        if (_cubePool == null)
            Debug.LogError($"CubePool not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombPool == null)
            Debug.LogError($"BombPool not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombSpawner == null)
            Debug.LogError($"BombSpawner not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombSpawner != null)
            _bombSpawner.OnSpawnerStateChanged += OnBombSpawnerStateChanged;
    }

    private void OnValidate()
    {
        UpdateBombTextVisibility(); // ДОБАВЛЕНО: постоянная проверка
    }

    private void OnDestroy()
    {
        if (_bombSpawner != null)
            _bombSpawner.OnSpawnerStateChanged -= OnBombSpawnerStateChanged;
    }

    private void Update()
    {
        UpdateCubeDisplay();
        UpdateBombDisplay();
        UpdateBombTextVisibility(); // ВАЖНО: постоянная проверка
    }

    private void OnBombSpawnerStateChanged(bool isSpawning)
    {
        UpdateBombTextVisibility();
        Debug.Log($"Bomb spawner state changed: {(isSpawning ? "ENABLED" : "DISABLED")}");
    }


    private void UpdateCubeDisplay()
    {
        if (_cubePool == null || _cubesInfoText == null)
            return;

        _stringBuilder.Clear();
        _stringBuilder.AppendLine("=== CUBES ===");
        _stringBuilder.AppendLine($"Active: {_cubePool.ActiveCount}");
        _stringBuilder.AppendLine($"In Pool: {_cubePool.InactiveCount}");
        _stringBuilder.AppendLine($"Total: {_cubePool.SpawnedCount}");

        _cubesInfoText.text = _stringBuilder.ToString();
    }

    private void UpdateBombDisplay()
    {
        if (_bombPool == null || _bombsInfoText == null)
            return;

        _stringBuilder.Clear();
        _stringBuilder.AppendLine("=== BOMBS ===");
        _stringBuilder.AppendLine($"Active: {_bombPool.ActiveCount}");
        _stringBuilder.AppendLine($"In Pool: {_bombPool.InactiveCount}");
        _stringBuilder.AppendLine($"Total Spawned: {_bombPool.SpawnedCount}");
        _stringBuilder.AppendLine($"Explosions: {_bombPool.TotalExplosions}");

        _bombsInfoText.text = _stringBuilder.ToString();
    }

    private void UpdateBombTextVisibility()
    {
        if (_bombsInfoText == null)
            return;

        //bool shouldShow = (_bombSpawner != null && _bombSpawner.IsSpawning) ||
        //                 (_bombPool != null && _bombPool.ActiveCount > 0);

        //_bombsInfoText.gameObject.SetActive(shouldShow);
        // ПРОСТАЯ И НАДЕЖНАЯ ПРОВЕРКА
        //bool spawnerEnabled = _bombSpawner != null && _bombSpawner.IsSpawning;
        //bool hasActiveBombs = _bombPool != null && _bombPool.ActiveCount > 0;
        //bool hasExplosions = _bombPool != null && _bombPool.TotalExplosions > 0;

        //bool shouldShow = spawnerEnabled || hasActiveBombs || hasExplosions;

        //if (_bombsInfoText.gameObject.activeSelf != shouldShow)
        //    Debug.Log($"Bomb Text Visibility: {shouldShow} (Spawner: {spawnerEnabled}, Active: {hasActiveBombs}, Explosions: {hasExplosions})");
        bool shouldShow = (_bombSpawner != null && _bombSpawner.IsSpawning) ||
                        (_bombPool != null && _bombPool.ActiveCount > 0) ||
                        (_bombPool != null && _bombPool.TotalExplosions > 0);

        _bombsInfoText.gameObject.SetActive(shouldShow);
    }
}