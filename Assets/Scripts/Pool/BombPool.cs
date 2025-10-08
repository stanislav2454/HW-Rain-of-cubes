using UnityEngine;

public class BombPool : Pool<Bomb>
{
    //[Header("Bomb Pool Specific Settings")]
    //[SerializeField] private bool _autoSpawnBombs = false;
    //[SerializeField] [Range(0, 9)] private float _autoSpawnInterval = 5f;

    private int _totalExplosions = 0;

    public int TotalExplosions => _totalExplosions;

    //protected override void Awake()
    //{
    //    base.Awake();
    //}

    // ПЕРЕОПРЕДЕЛЯЕМ метод получения объекта
    public new Bomb GetPooledObject()
    {
        Bomb bomb = base.GetPooledObject();
        if (bomb != null)
        {
            bomb.Initialize(); // ГАРАНТИРУЕМ инициализацию
        }
        return bomb;
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
    }

    [ContextMenu("ContextMenu /ExplodeAllBombs ( )")]
    public void ExplodeAllBombs()
    {
        var activeBombs = FindObjectsOfType<Bomb>();

        foreach (var bomb in activeBombs)
            if (bomb.gameObject.activeInHierarchy)
                bomb.Invoke("Explode", 0.1f);
                //bomb.Explode();
    }

    // ДОБАВЛЕНО: метод для получения расширенной статистики бомб
    public string GetBombExtendedStatistics()
    {
        return $"=== BOMBS EXTENDED STATS ===\n" +
               $"{GetFullStatistics()}\n" +
               $"Total Explosions: {_totalExplosions}\n" +
               $"Pool Capacity: {_defaultCapacity}/{_maxSize}";
    }
}