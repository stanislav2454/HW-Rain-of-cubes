using UnityEngine;

public class BombPool : Pool<Bomb>
{
    [Header("Bomb Pool Specific Settings")]
    [SerializeField] private bool _autoSpawnBombs = false;
    [SerializeField] [Range(0, 9)] private float _autoSpawnInterval = 5f;

    // private BombSpawner _spawner;
    private int _totalExplosions = 0;

    public int TotalExplosions => _totalExplosions;

    protected override void Awake()
    {
        base.Awake();

        //if (TryGetComponent(out _spawner) == false)
        //    _spawner = gameObject.AddComponent<BombSpawner>();

        //// Настраиваем спавнер
        //if (_spawner != null)
        //{
        //    _spawner.enabled = _autoSpawnBombs;
        //    if (_autoSpawnBombs)
        //    {
        //        // Устанавливаем интервал спавна для бомб
        //        var spawner = GetComponent<BombSpawner>();
        //        if (spawner != null)
        //        {
        //            // Можно настроить через reflection или добавить свойство в GenericSpawner
        //        }
        //    }
        //}
    }

    protected override Bomb CreatePooledObject()
    {
        Bomb bomb = base.CreatePooledObject();

        bomb.OnBombExploded += OnBombExploded;

        return bomb;
    }

    protected override void OnDestroyPooledObject(Bomb bomb)
    {
        if (bomb != null)
            bomb.OnBombExploded -= OnBombExploded;

        base.OnDestroyPooledObject(bomb);
    }

    private void OnBombExploded(Bomb bomb)
    {
        _totalExplosions++;
        Debug.Log($"Bomb exploded! Total explosions: {_totalExplosions}");
    }

    //public void SetAutoSpawningEnabled(bool enabled)
    //{
    //    _autoSpawnBombs = enabled;
    //    if (_spawner != null)
    //    {
    //        _spawner.enabled = enabled;
    //    }
    //}

    //public void SpawnBombAtPosition(Vector3 position)
    //{
    //    Bomb bomb = GetPooledObject();
    //    if (bomb != null)
    //    {
    //        bomb.transform.position = position;
    //        bomb.Initialize();
    //    }
    //}

    [ContextMenu("ContextMenu /ExplodeAllBombs ( )")]
    public void ExplodeAllBombs()
    {
        var activeBombs = FindObjectsOfType<Bomb>();

        foreach (var bomb in activeBombs)
            if (bomb.gameObject.activeInHierarchy)
                bomb.Invoke("Explode", 0.1f);
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