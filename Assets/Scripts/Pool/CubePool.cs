using UnityEngine;

public class CubePool : Pool<Cube>
{
    [Header("Cube Pool Specific Settings")]
    [SerializeField] private bool _enableSpawning = true;

    private CubeSpawner _spawner;

    protected override void Awake()
    {
        base.Awake();

        // Автоматически находим или добавляем спавнер
        if (TryGetComponent(out _spawner) == false)
        {
            _spawner = gameObject.AddComponent<CubeSpawner>();
        }

        // Настраиваем спавнер если он отключен
        if (_spawner != null)
        {
            _spawner.enabled = _enableSpawning;
        }
    }

    private void OnValidate()
    {
        //// В редакторе автоматически устанавливаем имя типа
        if (string.IsNullOrEmpty(ObjectType))
        {
            //_objectTypeName = "Cubes";
            //ObjectType = "Cubes";
        }
    }

    /// <summary>
    /// Включает или выключает спавн кубов
    /// </summary>
    public void SetSpawningEnabled(bool enabled)
    {
        _enableSpawning = enabled;

        if (_spawner != null)
        {
            _spawner.enabled = enabled;
        }
    }

    /// <summary>
    /// Возвращает все активные кубы в пул
    /// </summary>
    [ContextMenu("ReturnAllCubes")]
    public void ReturnAllCubes()
    {
        var activeCubes = FindObjectsOfType<Cube>();

        foreach (var cube in activeCubes)
        {
            if (cube.gameObject.activeInHierarchy)
            {
                ReturnToPool(cube);
            }
        }
    }

    /// <summary>
    /// Получает статистику по цветам кубов
    /// </summary>
    public string GetColorStatistics()
    {
        var activeCubes = FindObjectsOfType<Cube>();
        var colorCount = new System.Collections.Generic.Dictionary<Color, int>();

        foreach (var cube in activeCubes)
        {
            if (cube.gameObject.activeInHierarchy)
            {
                var renderer = cube.GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    Color color = renderer.material.color;
                    if (colorCount.ContainsKey(color))
                    {
                        colorCount[color]++;
                    }
                    else
                    {
                        colorCount[color] = 1;
                    }
                }
            }
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Cube Color Statistics:");
        foreach (var kvp in colorCount)
        {
            sb.AppendLine($"  Color {kvp.Key}: {kvp.Value} cubes");
        }

        return sb.ToString();
    }
}