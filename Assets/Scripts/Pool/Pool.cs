using UnityEngine;
using UnityEngine.Pool;

public class Pool<T> : MonoBehaviour, IPoolInfo where T : MonoBehaviour, IPoolable
{
    private const int MinPoolValue = 1;

    [SerializeField] protected int _defaultCapacity = 10;
    [SerializeField] protected int _maxSize = 20;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private T _prefab;
    [SerializeField] private string _objectTypeName;

    private IObjectPool<T> _pool;
    private int _spawnedCount;
    private int _createdCount;
    private int _activeCount; // ДОБАВЛЕНО: счетчик активных объектов

    public int SpawnedCount => _spawnedCount;
    public int CreatedCount => _createdCount;
    public int ActiveCount => _activeCount; // ИСПРАВЛЕНО: используем свой счетчик
    public int InactiveCount => _pool?.CountInactive ?? 0;
    public string ObjectType => _objectTypeName;

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(
            CreatePooledObject,
            GetFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            _collectionCheck,
            _defaultCapacity,
            _maxSize);
    }

    private void OnValidate()
    {
        _defaultCapacity = Mathf.Max(0, _defaultCapacity);
        _maxSize = Mathf.Max(MinPoolValue, _maxSize);

        if (_defaultCapacity > _maxSize)
            _defaultCapacity = _maxSize;

        if (string.IsNullOrEmpty(value: ObjectType))
            Debug.LogWarning($"ObjectType Name not set for {GetType().Name} on {gameObject.name}", this);
    }

    public T GetPooledObject()
    {
        if (_pool == null)
            return null;

        _spawnedCount++;
        _activeCount++; // УВЕЛИЧИВАЕМ при получении объекта
        return _pool.Get();
    }

    public void ReturnToPool(T obj)
    {
        if (_pool != null)
        {
            _pool.Release(obj);
            _activeCount--; // УМЕНЬШАЕМ при возврате объекта
            _activeCount = Mathf.Max(0, _activeCount); // Защита от отрицательных значений
        }
    }

    protected virtual void OnDestroyPooledObject(T obj) =>
        Destroy(obj.gameObject);

    protected virtual T CreatePooledObject()
    {
        T obj = Instantiate(_prefab, transform);
        obj.gameObject.SetActive(false);
        obj.SetPool(this);
        _createdCount++;

        return obj;
    }

    private void GetFromPool(T obj)
    {
        obj.gameObject.SetActive(true);

        // ВАЖНО: Вызываем Reset объекта при получении из пула
        if (obj is Cube cube)
            cube.ResetCube();
        else if (obj is Bomb bomb)
            bomb.ResetBomb();
    }

    private void OnReturnedToPool(T obj) =>
        obj.gameObject.SetActive(false);

    // ДОБАВЛЕНО: метод для получения полной статистики
    public string GetFullStatistics()
    {
        return $"{ObjectType} Statistics:\n" +
               $"  Total Spawned: {SpawnedCount}\n" +
               $"  Total Created: {CreatedCount}\n" +
               $"  Currently Active: {ActiveCount}\n" +
               $"  In Pool (Available): {InactiveCount}";
    }
}