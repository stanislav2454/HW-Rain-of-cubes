using UnityEngine;
using System.Text;
using TMPro;

public class PoolInfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private bool _showExtendedStats = true;

    [SerializeField] private CubePool _cubePool;
    [SerializeField] private BombPool _bombPool;
    private StringBuilder _stringBuilder = new StringBuilder();

    private void Start()
    {
        if (_cubePool == null)
            Debug.LogError($"CubePool not found for {GetType().Name} on {gameObject.name}", this);

        if (_bombPool == null)
            Debug.LogError($"BombPool not found for {GetType().Name} on {gameObject.name}", this);
        // Найти пулы в сцене
        //_cubePool = FindObjectOfType<Pool<CubeController>>();
        //_bombPool = FindObjectOfType<Pool<Bomb>>();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        _stringBuilder.Clear();

        _stringBuilder.AppendLine("=== POOL STATISTICS ===");

        if (_cubePool != null)
        {
            _stringBuilder.AppendLine("CUBES:");
            _stringBuilder.AppendLine($"  Spawned: {_cubePool.SpawnedCount}");
            _stringBuilder.AppendLine($"  Created: {_cubePool.CreatedCount}");
            _stringBuilder.AppendLine($"  Active: {_cubePool.ActiveCount}");
            _stringBuilder.AppendLine();
        }

        if (_bombPool != null)
        {
            _stringBuilder.AppendLine("BOMBS:");
            _stringBuilder.AppendLine($"  Spawned: {_bombPool.SpawnedCount}");
            _stringBuilder.AppendLine($"  Created: {_bombPool.CreatedCount}");
            _stringBuilder.AppendLine($"  Active: {_bombPool.ActiveCount}");
        }

        _infoText.text = _stringBuilder.ToString();
    }
}