using UnityEngine;

public class BombPool : Pool<Bomb>
{
    [Header("Bomb Pool Specific Settings")]
    [SerializeField] private bool _autoSpawnBombs = false;
    [SerializeField] [Range(0, 9)] private float _autoSpawnInterval = 5f;

    private BombSpawner _spawner;
    private int _totalExplosions = 0;

    public int TotalExplosions => _totalExplosions;

    protected override void Awake()
    {
        base.Awake();

        // Автоматически находим или добавляем спавнер
        if (!TryGetComponent(out _spawner))
        {
            _spawner = gameObject.AddComponent<BombSpawner>();
        }

        // Настраиваем спавнер
        if (_spawner != null)
        {
            _spawner.enabled = _autoSpawnBombs;
            if (_autoSpawnBombs)
            {
                // Устанавливаем интервал спавна для бомб
                var spawner = GetComponent<BombSpawner>();
                if (spawner != null)
                {
                    // Можно настроить через reflection или добавить свойство в GenericSpawner
                }
            }
        }
    }

    private void OnValidate()
    {
        // В редакторе автоматически устанавливаем имя типа
        if (string.IsNullOrEmpty(value: ObjectType))
        {
            //ObjectType = "Bombs";
        }
    }

    protected override Bomb CreatePooledObject()
    {
        Bomb bomb = base.CreatePooledObject();

        // Подписываемся на событие взрыва бомбы
        bomb.OnBombExploded += OnBombExploded;

        return bomb;
    }

    protected override void OnDestroyPooledObject(Bomb bomb)
    {
        // Отписываемся от события перед уничтожением
        if (bomb != null)
        {
            bomb.OnBombExploded -= OnBombExploded;
        }

        base.OnDestroyPooledObject(bomb);
    }

    private void OnBombExploded(Bomb bomb)
    {
        _totalExplosions++;
        Debug.Log($"Bomb exploded! Total explosions: {_totalExplosions}");
    }

    /// <summary>
    /// Включает или выключает автоматический спавн бомб
    /// </summary>
    public void SetAutoSpawningEnabled(bool enabled)
    {
        _autoSpawnBombs = enabled;
        if (_spawner != null)
        {
            _spawner.enabled = enabled;
        }
    }

    /// <summary>
    /// Создает бомбу в указанной позиции
    /// </summary>
    public void SpawnBombAtPosition(Vector3 position)
    {
        Bomb bomb = GetPooledObject();
        if (bomb != null)
        {
            bomb.transform.position = position;
            bomb.Initialize();
        }
    }

    /// <summary>
    /// Взрывает все активные бомбы немедленно
    /// </summary>
    public void ExplodeAllBombs()
    {
        var activeBombs = FindObjectsOfType<Bomb>();
        foreach (var bomb in activeBombs)
        {
            if (bomb.gameObject.activeInHierarchy)
            {
                // Вызываем взрыв немедленно
                bomb.Invoke("Explode", 0.1f); // Небольшая задержка для визуального эффекта
            }
        }
    }

    /// <summary>
    /// Возвращает расширенную статистику по бомбам
    /// </summary>
    public string GetExtendedStatistics()
    {
        return $"{ObjectType} Statistics:\n" +
               $"  Total Spawned: {SpawnedCount}\n" +
               $"  Total Created: {CreatedCount}\n" +
               $"  Currently Active: {ActiveCount}\n" +
               $"  Total Explosions: {_totalExplosions}";
    }
}